using System;
using LegendaryTools;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace LegedaryTools.Mothership.Iap
{
    public class UnityDetailedStoreListener : IDetailedStoreListener
    {
        public event Action<IStoreController, IExtensionProvider> OnInitialize;
        public event Action<InitializationFailureReason, string> OnInitializeFail;
        public event Func<PurchaseEventArgs, PurchaseProcessingResult> ProcessPurchased;
        public event Action<Product, PurchaseFailureReason> OnPurchaseFail;
        public event Action<Product, PurchaseFailureDescription> OnPurchaseFailedDesc;
        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            OnInitialize?.Invoke(controller, extensions);
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        { } //Super seeded by OnInitializeFailed(InitializationFailureReason error, string message)

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            OnInitializeFail?.Invoke(error, message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if (ProcessPurchased == null)
            {
                if(Mothership.LogLevel.HasFlags(MothershipLogLevel.Error))
                    Debug.LogError("[UnityDetailedStoreListener:ProcessPurchase] ProcessPurchased callback cannot be null.");
                return default;
            }
            return ProcessPurchased(purchaseEvent);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            OnPurchaseFail?.Invoke(product, failureReason);
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            OnPurchaseFailedDesc?.Invoke(product, failureDescription);
        }
    }
}