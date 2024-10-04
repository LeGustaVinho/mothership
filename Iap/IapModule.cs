using System;
using System.Collections.Generic;
using UnityEngine;

namespace LegedaryTools.Mothership.Iap
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Modules/IapModule", fileName = "IapModule", order = 0)]
    public class IapModule : MothershipModule, IIapProvider
    {
        public Func<IapProductConfig, PurchaseProcessingResultWrapper> ProcessPurchasedProduct
        {
            get
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    return iapProvider.ProcessPurchasedProduct;
                }
                return null;
            }
            set
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.ProcessPurchasedProduct = value;
                }
            }
        }
        public Func<IapProductConfig, bool> ValidateIfProductWasConsumed
        {
            get
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    return iapProvider.ValidateIfProductWasConsumed;
                }
                return null;
            }
            set
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.ValidateIfProductWasConsumed = value;
                }
            }
        }
        public List<IapProductConfig> Products
        {
            get
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    return iapProvider.Products;
                }
                return new List<IapProductConfig>();
            }
        }
        public event Action OnInitializationCompleted
        {
            add
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnInitializationCompleted += value;
                }
            }
            remove
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnInitializationCompleted -= value;
                }
            }
        }
        public event Action<InitializationFailureReasonWrapper, string> OnInitializeFailed
        {
            add
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnInitializeFailed += value;
                }
            }
            remove
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnInitializeFailed -= value;
                }
            }
        }
        public event Action<IapProductConfig, PurchaseProcessingResultWrapper> OnPurchaseResponse
        {
            add
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnPurchaseResponse += value;
                }
            }
            remove
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnPurchaseResponse -= value;
                }
            }
        }
        public event Action<IapProductConfig, PurchaseFailureReasonWrapper, string> OnPurchaseFailed
        {
            add
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnPurchaseFailed += value;
                }
            }
            remove
            {
                foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
                {
                    iapProvider.OnPurchaseFailed -= value;
                }
            }
        }
        
        public void InitiatePurchase(IapProductConfig product, string payload = "")
        {
            foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
            {
                iapProvider.InitiatePurchase(product, payload);
            }
        }

        public void InitiatePurchase(string productId, string payload = "")
        {
            foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
            {
                iapProvider.InitiatePurchase(productId, payload);
            }
        }

        public void ConfirmPendingPurchase(IapProductConfig product)
        {
            foreach (IapProvider iapProvider in SelectProvidersByBehaviour<IapProvider>())
            {
                iapProvider.ConfirmPendingPurchase(product);
            }
        }
    }
}