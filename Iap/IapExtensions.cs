using System;
using UnityEngine.Purchasing;

namespace LegedaryTools.Mothership.Iap
{
    public static class IapExtensions
    {
        public static ProductTypeWrapper ToProductTypeWrapper(this ProductType type)
        {
            return type switch
            {
                ProductType.Consumable => ProductTypeWrapper.Consumable,
                ProductType.NonConsumable => ProductTypeWrapper.NonConsumable,
                ProductType.Subscription => ProductTypeWrapper.Subscription,
                _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unhandled ProductType: {type}")
            };
        }
        
        public static ProductType ToProductType(this ProductTypeWrapper wrapper)
        {
            return wrapper switch
            {
                ProductTypeWrapper.Consumable => ProductType.Consumable,
                ProductTypeWrapper.NonConsumable => ProductType.NonConsumable,
                ProductTypeWrapper.Subscription => ProductType.Subscription,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), $"Unhandled ProductTypeWrapper: {wrapper}")
            };
        }

        public static PayoutTypeWrapper ToPayoutTypeWrapper(this PayoutType type)
        {
            return type switch
            {
                PayoutType.Currency => PayoutTypeWrapper.Currency,
                PayoutType.Item => PayoutTypeWrapper.Item,
                PayoutType.Other => PayoutTypeWrapper.Other,
                PayoutType.Resource => PayoutTypeWrapper.Resource,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static PayoutType ToPayoutType(this PayoutTypeWrapper wrapper)
        {
            return wrapper switch
            {
                PayoutTypeWrapper.Currency => PayoutType.Currency,
                PayoutTypeWrapper.Item => PayoutType.Item,
                PayoutTypeWrapper.Other => PayoutType.Other,
                PayoutTypeWrapper.Resource => PayoutType.Resource,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), wrapper, null)
            };
        }

        public static PurchaseFailureReason ToPurchaseFailureReason(this PurchaseFailureReasonWrapper wrapper)
        {
            return wrapper switch
            {
                PurchaseFailureReasonWrapper.PurchasingUnavailable => PurchaseFailureReason.PurchasingUnavailable,
                PurchaseFailureReasonWrapper.ExistingPurchasePending => PurchaseFailureReason.ExistingPurchasePending,
                PurchaseFailureReasonWrapper.ProductUnavailable => PurchaseFailureReason.ProductUnavailable,
                PurchaseFailureReasonWrapper.SignatureInvalid => PurchaseFailureReason.SignatureInvalid,
                PurchaseFailureReasonWrapper.UserCancelled => PurchaseFailureReason.UserCancelled,
                PurchaseFailureReasonWrapper.PaymentDeclined => PurchaseFailureReason.PaymentDeclined,
                PurchaseFailureReasonWrapper.DuplicateTransaction => PurchaseFailureReason.DuplicateTransaction,
                PurchaseFailureReasonWrapper.Unknown => PurchaseFailureReason.Unknown,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), wrapper, null)
            };
        }
        
        public static PurchaseFailureReasonWrapper ToPurchaseFailureReasonWrapper(this PurchaseFailureReason wrapper)
        {
            return wrapper switch
            {
                PurchaseFailureReason.PurchasingUnavailable => PurchaseFailureReasonWrapper.PurchasingUnavailable,
                PurchaseFailureReason.ExistingPurchasePending => PurchaseFailureReasonWrapper.ExistingPurchasePending,
                PurchaseFailureReason.ProductUnavailable => PurchaseFailureReasonWrapper.ProductUnavailable,
                PurchaseFailureReason.SignatureInvalid => PurchaseFailureReasonWrapper.SignatureInvalid,
                PurchaseFailureReason.UserCancelled => PurchaseFailureReasonWrapper.UserCancelled,
                PurchaseFailureReason.PaymentDeclined => PurchaseFailureReasonWrapper.PaymentDeclined,
                PurchaseFailureReason.DuplicateTransaction => PurchaseFailureReasonWrapper.DuplicateTransaction,
                PurchaseFailureReason.Unknown => PurchaseFailureReasonWrapper.Unknown,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), wrapper, null)
            };
        }
        
        public static PurchaseProcessingResult ToPurchaseProcessingResult(this PurchaseProcessingResultWrapper wrapper)
        {
            return wrapper switch
            {
                PurchaseProcessingResultWrapper.Complete => PurchaseProcessingResult.Complete,
                PurchaseProcessingResultWrapper.Pending => PurchaseProcessingResult.Pending,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), wrapper, null)
            };
        }
        
        public static PurchaseProcessingResultWrapper ToPurchaseProcessingResultWrapper(this PurchaseProcessingResult wrapper)
        {
            return wrapper switch
            {
                PurchaseProcessingResult.Complete => PurchaseProcessingResultWrapper.Complete,
                PurchaseProcessingResult.Pending => PurchaseProcessingResultWrapper.Pending,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), wrapper, null)
            };
        }
        
        public static InitializationFailureReason ToInitializationFailureReason(this InitializationFailureReasonWrapper wrapper)
        {
            return wrapper switch
            {
                InitializationFailureReasonWrapper.PurchasingUnavailable => InitializationFailureReason.PurchasingUnavailable,
                InitializationFailureReasonWrapper.NoProductsAvailable => InitializationFailureReason.NoProductsAvailable,
                InitializationFailureReasonWrapper.AppNotKnown => InitializationFailureReason.AppNotKnown,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), wrapper, null)
            };
        }
        
        public static InitializationFailureReasonWrapper ToInitializationFailureReasonWrapper(this InitializationFailureReason wrapper)
        {
            return wrapper switch
            {
                InitializationFailureReason.PurchasingUnavailable => InitializationFailureReasonWrapper.PurchasingUnavailable,
                InitializationFailureReason.NoProductsAvailable => InitializationFailureReasonWrapper.NoProductsAvailable,
                InitializationFailureReason.AppNotKnown => InitializationFailureReasonWrapper.AppNotKnown,
                _ => throw new ArgumentOutOfRangeException(nameof(wrapper), wrapper, null)
            };
        }
    }
}