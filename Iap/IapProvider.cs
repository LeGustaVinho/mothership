using System;
using System.Collections.Generic;

namespace LegedaryTools.Mothership.Iap
{
    public abstract class IapProvider : InitalizeableScriptableObject, IIapStore
    {
        public IapProductCatalogProviderConfig ProductCatalogProvider;
        public abstract List<IapProductConfig> Products { get; }
        public abstract void InitiatePurchase(IapProductConfig product, string payload = "");
        public abstract void InitiatePurchase(string productId, string payload = "");
        public abstract void ConfirmPendingPurchase(IapProductConfig product);
        public abstract PurchaseProcessingResultWrapper ProcessPurchasedProduct(IapProductConfig iapProductConfig);
        public abstract bool ValidateIfProductWasConsumed(IapProductConfig iapProductConfig);
        public abstract event Action OnInitializationCompleted;
        public abstract event Action<InitializationFailureReasonWrapper, string> OnInitializeFailed;
        public abstract event Action<IapProductConfig, PurchaseProcessingResultWrapper> OnPurchaseResponse;
        public abstract event Action<IapProductConfig, PurchaseFailureReasonWrapper, string> OnPurchaseFailed;
    }
}