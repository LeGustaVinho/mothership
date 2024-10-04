using System;
using System.Collections.Generic;

namespace LegedaryTools.Mothership.Iap
{
    public abstract class IapProvider : BaseProvider, IIapProvider
    {
        public IapProductCatalogProviderConfig ProductCatalogProvider;
        public abstract Func<IapProductConfig, PurchaseProcessingResultWrapper> ProcessPurchasedProduct { get; set; }
        public abstract Func<IapProductConfig, bool> ValidateIfProductWasConsumed { get; set; }
        public abstract List<IapProductConfig> Products { get; }
        public abstract void InitiatePurchase(IapProductConfig product, string payload = "");
        public abstract void InitiatePurchase(string productId, string payload = "");
        public abstract void ConfirmPendingPurchase(IapProductConfig product);
        public abstract event Action OnInitializationCompleted;
        public abstract event Action<InitializationFailureReasonWrapper, string> OnInitializeFailed;
        public abstract event Action<IapProductConfig, PurchaseProcessingResultWrapper> OnPurchaseResponse;
        public abstract event Action<IapProductConfig, PurchaseFailureReasonWrapper, string> OnPurchaseFailed;
    }
}