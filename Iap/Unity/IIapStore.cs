using System;
using System.Collections.Generic;

namespace LegedaryTools.Mothership.Iap
{
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
}