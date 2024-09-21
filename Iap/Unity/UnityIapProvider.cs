using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LegendaryTools;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace LegedaryTools.Mothership.Iap
{
    public enum PurchaseFailureReasonWrapper
    {
        PurchasingUnavailable,
        ExistingPurchasePending,
        ProductUnavailable,
        SignatureInvalid,
        UserCancelled,
        PaymentDeclined,
        DuplicateTransaction,
        Unknown
    }

    public enum PurchaseProcessingResultWrapper
    {
        Complete,
        Pending
    }
    
    public enum InitializationFailureReasonWrapper
    {
        PurchasingUnavailable,
        NoProductsAvailable,
        AppNotKnown
    }
    
    public interface IIapStore
    {
        List<IapProductConfig> Products { get; }
        void InitiatePurchase(IapProductConfig product, string payload = "");
        void InitiatePurchase(string productId, string payload = "");
        void ConfirmPendingPurchase(IapProductConfig product);
        PurchaseProcessingResultWrapper ProcessPurchasedProduct(IapProductConfig iapProductConfig);
        bool ValidateIfProductWasConsumed(IapProductConfig iapProductConfig);

        event Action OnInitializationCompleted;
        event Action<InitializationFailureReasonWrapper, string> OnInitializeFailed;
        event Action<IapProductConfig, PurchaseProcessingResultWrapper> OnPurchaseResponse;
        event Action<IapProductConfig, PurchaseFailureReasonWrapper, string> OnPurchaseFailed;
        
    }
    
    [CreateAssetMenu(menuName = "Tools/Mothership/UnityIapProvider", fileName = "UnityIapProvider", order = 0)]
    public class UnityIapProvider : IapProvider, IIapStore
    {
        public string Environment = "production";
        public List<IapProductConfig> Products => productCatalog.Products;
        public IExtensionProvider UnityExtensionProvider { get; private set; }

        private IapProductCatalogConfig productCatalog;
        private IStoreController unityStoreController;
        private UnityDetailedStoreListener unityDetailedStoreListener;
        private Bictionary<IapProductConfig, Product> productTable = new Bictionary<IapProductConfig, Product>();
        
        public event Action OnInitializationCompleted;
        public event Action<InitializationFailureReasonWrapper, string> OnInitializeFailed;
        public event Action<IapProductConfig, PurchaseProcessingResultWrapper> OnPurchaseResponse;
        public event Action<IapProductConfig, PurchaseFailureReasonWrapper, string> OnPurchaseFailed;
        

        public override async Task Initialize()
        {
            try
            {
                InitializationOptions options = new InitializationOptions().SetEnvironmentName(Environment);
                await UnityServices.InitializeAsync(options);
                productCatalog = await ProductCatalogProvider.GetProductCatalog();
                productCatalog.Touch();
                
                ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                foreach (IapProductConfig productConfig in productCatalog.Products)
                {
                    productConfig.Touch();
                    IDs ids = new IDs();
                    foreach (KeyValuePair<IapStoreConfig, string> pair in productConfig.StoreIds)
                    {
                        ids.Add(pair.Key.Name, pair.Value);
                    }
                    
                    builder.AddProduct(productConfig.Guid,
                        productConfig.Type.ToProductType(),
                        ids,
                        productConfig.PayoutDefinitions.ConvertAll<PayoutDefinition>((item) => item));
                }

                unityDetailedStoreListener.OnInitialize += OnInitialize;
                unityDetailedStoreListener.OnInitializeFail += OnInitializeFail;
                unityDetailedStoreListener.ProcessPurchased += ProcessPurchased;
                unityDetailedStoreListener.OnPurchaseFail += OnPurchaseFail;
                unityDetailedStoreListener.OnPurchaseFailedDesc += OnPurchaseFailedDesc;
                
                UnityPurchasing.Initialize (unityDetailedStoreListener, builder);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
        
        public void InitiatePurchase(IapProductConfig productConfig, string payload = "")
        {
            if (productConfig == null)
            {
                Debug.LogError("[UnityIapProvider:InitiatePurchase] Product cannot be null.");
            }

            if (!productTable.TryGetValue(productConfig, out Product nativeProduct))
            {
                Debug.LogError($"[UnityIapProvider:InitiatePurchase] Native product was not found.");
                return;
            }
            
            unityStoreController.InitiatePurchase(nativeProduct, payload);
        }

        public void InitiatePurchase(string productId, string payload = "")
        {
            Product product = unityStoreController.products.WithID(productId);
            if (product == null)
            {
                Debug.LogError($"[UnityIapProvider:InitiatePurchase({productId})] Native product was not found.");
                return;
            }
            
            unityStoreController.InitiatePurchase(product, payload);
        }

        public void ConfirmPendingPurchase(IapProductConfig productConfig)
        {
            if (!productTable.TryGetValue(productConfig, out Product nativeProduct))
            {
                Debug.LogError($"[UnityIapProvider:InitiatePurchase] Native product was not found.");
                return;
            }
            
            unityStoreController.ConfirmPendingPurchase(nativeProduct);
        }

        public override void Dispose()
        {
            productTable.Clear();
            unityDetailedStoreListener.OnInitialize -= OnInitialize;
            unityDetailedStoreListener.OnInitializeFail -= OnInitializeFail;
            unityDetailedStoreListener.ProcessPurchased -= ProcessPurchased;
            unityDetailedStoreListener.OnPurchaseFail -= OnPurchaseFail;
            unityDetailedStoreListener.OnPurchaseFailedDesc -= OnPurchaseFailedDesc;
            unityDetailedStoreListener = null;
            unityStoreController = null;
            UnityExtensionProvider = null;
            productCatalog = null;
        }

        public virtual PurchaseProcessingResultWrapper ProcessPurchasedProduct(IapProductConfig iapProductConfig)
        {
            switch (iapProductConfig.Type)
            {
                case ProductTypeWrapper.Consumable:
                {
                    if (iapProductConfig.AutoConsume) return PurchaseProcessingResultWrapper.Complete;
                    return ValidateIfProductWasConsumed(iapProductConfig)
                        ? PurchaseProcessingResultWrapper.Complete
                        : PurchaseProcessingResultWrapper.Pending;
                }
                case ProductTypeWrapper.NonConsumable: return PurchaseProcessingResultWrapper.Complete;
                case ProductTypeWrapper.Subscription: return PurchaseProcessingResultWrapper.Complete;
                default: return PurchaseProcessingResultWrapper.Pending;
            }
        }

        public virtual bool ValidateIfProductWasConsumed(IapProductConfig iapProductConfig)
        {
            return true;
        }
        
        private void OnInitialize(IStoreController storeController, IExtensionProvider extensionProvider)
        {
            unityStoreController = storeController;
            UnityExtensionProvider = extensionProvider;

            HandleProductsWhileInInitialization();
        }

        private void HandleProductsWhileInInitialization()
        {
            HashSet<ProductDefinition> missingProducts = new HashSet<ProductDefinition>();
            foreach (IapProductConfig product in productCatalog.Products)
            {
                Product nativeProduct = unityStoreController.products.WithID(product.Guid);
                if (nativeProduct == null)
                {
                    missingProducts.Add(new ProductDefinition(product.Guid, product.Type.ToProductType()));
                }
                else
                {
                    productTable.AddOrUpdate(product, nativeProduct);
                }
            }

            if (missingProducts.Count > 0)
            {
                unityStoreController.FetchAdditionalProducts(missingProducts, HandleProductsWhileInInitialization, 
                    OnErrorMissingProducts);
            }
            else
            {
                IsInitialized = true;
                OnInitializationCompleted?.Invoke();
            }
        }

        private void OnErrorMissingProducts(InitializationFailureReason initializationFailureReason, string failureReason)
        {
            Debug.LogError($"[UnityIapProvider:OnInitializeFail] {initializationFailureReason}, reason: {failureReason}");
        }

        private void OnInitializeFail(InitializationFailureReason initializationFailureReason, string failureReason)
        {
            Debug.LogError($"[UnityIapProvider:OnInitializeFail] {initializationFailureReason}, reason: {failureReason}");
            OnInitializeFailed?.Invoke(initializationFailureReason.ToInitializationFailureReasonWrapper(), failureReason);
        }

        private PurchaseProcessingResult ProcessPurchased(PurchaseEventArgs purchaseEventArgs)
        {
            if (!TryGetProductConfig(purchaseEventArgs.purchasedProduct, out IapProductConfig iapProductConfig))
            {
                Debug.LogError($"[UnityIapProvider:ProcessPurchased] Iap product was not found.");
                return PurchaseProcessingResult.Pending;
            }

            PurchaseProcessingResult result = ProcessPurchasedProduct(iapProductConfig).ToPurchaseProcessingResult();
            OnPurchaseResponse?.Invoke(iapProductConfig, result.ToPurchaseProcessingResultWrapper());
            return result;
        }

        private bool TryGetProductConfig(Product product, out IapProductConfig iapProductConfig)
        {
            if (!productTable.TryGetValue(product, out iapProductConfig))
            {
                Debug.LogWarning($"[UnityIapProvider:FindIapProductConfig] Iap product {product.definition.id} was not found in productTable, trying another method ...");

                if (productCatalog == null)
                {
                    Debug.LogError($"[UnityIapProvider:FindIapProductConfig] Product catalog is null.");
                }

                iapProductConfig = productCatalog.Products.Find(item => item.Guid == product.definition.id);
                if (iapProductConfig == null)
                {
                    Debug.LogError($"[UnityIapProvider:FindIapProductConfig] Iap product was not found.");
                }
            }

            return iapProductConfig;
        }

        private void OnPurchaseFail(Product product, PurchaseFailureReason purchaseFailureReason)
        { }

        private void OnPurchaseFailedDesc(Product product, PurchaseFailureDescription purchaseFailureDescription)
        {
            if (!TryGetProductConfig(product, out IapProductConfig productConfig))
            {
                Debug.LogError($"[UnityIapProvider:ProcessPurchased] Iap product was not found.");
            }
            
            OnPurchaseFailed?.Invoke(productConfig, purchaseFailureDescription.reason.ToPurchaseFailureReasonWrapper(), 
                purchaseFailureDescription.message);
        }
    }
}