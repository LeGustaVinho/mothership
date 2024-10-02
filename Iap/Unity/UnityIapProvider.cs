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
    [CreateAssetMenu(menuName = "Tools/Mothership/UnityIapProvider", fileName = "UnityIapProvider", order = 0)]
    public class UnityIapProvider : IapProvider
    {
        public string Environment = "production";
        public override List<IapProductConfig> Products => productCatalog.Products;
        public IExtensionProvider UnityExtensionProvider { get; private set; }

        [NonSerialized] private IapProductCatalogConfig productCatalog;
        [NonSerialized] private IStoreController unityStoreController;
        [NonSerialized] private UnityDetailedStoreListener unityDetailedStoreListener;
        [NonSerialized] private readonly Bictionary<IapProductConfig, Product> productTable = 
            new Bictionary<IapProductConfig, Product>();
        
        public override event Action OnInitializationCompleted;
        public override event Action<InitializationFailureReasonWrapper, string> OnInitializeFailed;
        public override event Action<IapProductConfig, PurchaseProcessingResultWrapper> OnPurchaseResponse;
        public override event Action<IapProductConfig, PurchaseFailureReasonWrapper, string> OnPurchaseFailed;

        public override async Task Initialize()
        {
            if (!Enabled) return;
            if (IsInitialized) return;
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(Initialize)}] Initializing ....");
            
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
                if (Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                {
                    Debug.LogError($"[{nameof(UnityIapProvider)}:{nameof(Initialize)}] {this.GetType()} initialization error.");
                    Debug.LogException(exception);
                }
            }
        }

        public override async void OnEnableChange(bool newMode)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(OnEnableChange)}] state changed to {newMode}");
            
            if (newMode)
            {
                await Initialize();
            }
            else
            {
                Dispose();
                IsInitialized = false;
            }
        }

        public override void InitiatePurchase(IapProductConfig productConfig, string payload = "")
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(InitiatePurchase)}] Product {productConfig.name}");
            
            if (productConfig == null)
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                    Debug.LogError("[UnityIapProvider:InitiatePurchase] Product cannot be null.");
            }

            if (!productTable.TryGetValue(productConfig, out Product nativeProduct))
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                    Debug.LogError($"[UnityIapProvider:InitiatePurchase] Native product was not found.");
                return;
            }
            
            unityStoreController.InitiatePurchase(nativeProduct, payload);
        }

        public override void InitiatePurchase(string productId, string payload = "")
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(InitiatePurchase)}] Product id {productId}");
            
            Product product = unityStoreController.products.WithID(productId);
            if (product == null)
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                    Debug.LogError($"[UnityIapProvider:InitiatePurchase({productId})] Native product was not found.");
                return;
            }
            
            unityStoreController.InitiatePurchase(product, payload);
        }

        public override void ConfirmPendingPurchase(IapProductConfig productConfig)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(ConfirmPendingPurchase)}] Product {productConfig.name}");
            
            if (!productTable.TryGetValue(productConfig, out Product nativeProduct))
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                    Debug.LogError($"[UnityIapProvider:InitiatePurchase] Native product was not found.");
                return;
            }
            
            unityStoreController.ConfirmPendingPurchase(nativeProduct);
        }

        public override void Dispose()
        {
            if (!IsInitialized) return;
            
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
            
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(Dispose)}]");
        }

        public override PurchaseProcessingResultWrapper ProcessPurchasedProduct(IapProductConfig iapProductConfig)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(ProcessPurchasedProduct)}] Processing {iapProductConfig.name}");
            
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

        public override bool ValidateIfProductWasConsumed(IapProductConfig iapProductConfig)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(ValidateIfProductWasConsumed)}] Validated {iapProductConfig.name}");
            
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
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(HandleProductsWhileInInitialization)}]");
            
            HashSet<ProductDefinition> missingProducts = new HashSet<ProductDefinition>();
            foreach (IapProductConfig product in productCatalog.Products)
            {
                Product nativeProduct = unityStoreController.products.WithID(product.Guid);
                if (nativeProduct == null)
                {
                    if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Warning))
                        Debug.LogWarning($"[{nameof(UnityIapProvider)}:{nameof(HandleProductsWhileInInitialization)}] Missing native product with name {product.name}");
                    
                    missingProducts.Add(new ProductDefinition(product.Guid, product.Type.ToProductType()));
                }
                else
                {
                    productTable.AddOrUpdate(product, nativeProduct);
                }
            }

            if (missingProducts.Count > 0)
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Warning))
                    Debug.LogWarning($"[{nameof(UnityIapProvider)}:{nameof(HandleProductsWhileInInitialization)}] Missing products count {missingProducts.Count}, fetching again ...");
                
                unityStoreController.FetchAdditionalProducts(missingProducts, HandleProductsWhileInInitialization, 
                    OnErrorMissingProducts);
            }
            else
            {
                IsInitialized = true;
                OnInitializationCompleted?.Invoke();
                
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Info))
                    Debug.Log($"[{nameof(UnityIapProvider)}:{nameof(OnInitialize)}] Initialized.");
            }
        }

        private void OnErrorMissingProducts(InitializationFailureReason initializationFailureReason, string failureReason)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                Debug.LogError($"[UnityIapProvider:{nameof(OnErrorMissingProducts)}] {initializationFailureReason}, reason: {failureReason}");
        }

        private void OnInitializeFail(InitializationFailureReason initializationFailureReason, string failureReason)
        {
            if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                Debug.LogError($"[UnityIapProvider:OnInitializeFail] {initializationFailureReason}, reason: {failureReason}");
            OnInitializeFailed?.Invoke(initializationFailureReason.ToInitializationFailureReasonWrapper(), failureReason);
        }

        private PurchaseProcessingResult ProcessPurchased(PurchaseEventArgs purchaseEventArgs)
        {
            if (!TryGetProductConfig(purchaseEventArgs.purchasedProduct, out IapProductConfig iapProductConfig))
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
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
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Warning))
                    Debug.LogWarning($"[UnityIapProvider:{nameof(TryGetProductConfig)}] Iap product {product.definition.id} was not found in productTable, trying another method ...");

                if (productCatalog == null)
                {
                    if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                        Debug.LogError($"[UnityIapProvider:{nameof(TryGetProductConfig)}] Product catalog is null.");
                }

                iapProductConfig = productCatalog.Products.Find(item => item.Guid == product.definition.id);
                if (iapProductConfig == null)
                {
                    if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                        Debug.LogError($"[UnityIapProvider:{nameof(TryGetProductConfig)}] Iap product was not found.");
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
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                    Debug.LogError($"[UnityIapProvider:{nameof(OnPurchaseFailedDesc)}] Iap product was not found.");
            }
            
            OnPurchaseFailed?.Invoke(productConfig, purchaseFailureDescription.reason.ToPurchaseFailureReasonWrapper(), 
                purchaseFailureDescription.message);
        }
    }
}