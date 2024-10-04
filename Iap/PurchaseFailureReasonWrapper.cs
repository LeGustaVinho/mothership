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
}