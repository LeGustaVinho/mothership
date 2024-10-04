using System;
using System.Collections.Generic;

namespace LegedaryTools.Mothership.Iap
{
    public interface IIapProvider : IInitalizeable
    {
        Func<IapProductConfig, PurchaseProcessingResultWrapper> ProcessPurchasedProduct { get; set; }
        Func<IapProductConfig, bool> ValidateIfProductWasConsumed { get; set; }
        List<IapProductConfig> Products { get; }
        void InitiatePurchase(IapProductConfig product, string payload = "");
        void InitiatePurchase(string productId, string payload = "");
        void ConfirmPendingPurchase(IapProductConfig product);
        event Action OnInitializationCompleted;
        event Action<InitializationFailureReasonWrapper, string> OnInitializeFailed;
        event Action<IapProductConfig, PurchaseProcessingResultWrapper> OnPurchaseResponse;
        event Action<IapProductConfig, PurchaseFailureReasonWrapper, string> OnPurchaseFailed;
    }
}