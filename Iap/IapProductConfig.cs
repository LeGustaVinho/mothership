using System;
using System.Collections.Generic;
using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership.Iap
{
    public enum ProductTypeWrapper
    {
        Consumable,
        NonConsumable,
        Subscription
    }
    
    public enum PayoutTypeWrapper
    {
        Other,
        Currency,
        Item,
        Resource
    }

    public interface IPayoutDefinitionWrapper
    {
        PayoutTypeWrapper Type { get; }
        string Subtype { get; }
        double Quantity { get; }
    }

    [Serializable]
    public class PayoutDefinitionWrapper : IPayoutDefinitionWrapper
    {
        [SerializeField] private PayoutTypeWrapper type;
        [SerializeField] private string subtype;
        [SerializeField] private double quantity;
        [SerializeField] private string data;

        public PayoutTypeWrapper Type
        {
            get => type;
            set => type = value;
        }
        public string Subtype
        {
            get => subtype;
            set => subtype = value;
        }
        public double Quantity
        {
            get => quantity;
            set => quantity = value;
        }

        public string Data
        {
            get => data;
            set => data = value;
        }

        public PayoutDefinitionWrapper(PayoutTypeWrapper type, string subtype, double quantity, string data)
        {
            this.type = type;
            this.subtype = subtype;
            this.quantity = quantity;
            this.data = data;
        }

        #if ENABLE_UNITY_IAP

        #endif
    }
    
    
    [CreateAssetMenu(menuName = "Tools/Mothership/Iap/IapProductConfig", fileName = "IapProductConfig", order = 0)]
    public class IapProductConfig : UniqueScriptableObject
    {
        public ProductTypeWrapper Type;
        public bool IsConsumable => Type == ProductTypeWrapper.Consumable;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf("IsConsumable")]
#endif
        public bool AutoConsume;
        public Dictionary<IapStoreConfig, string> StoreIds = new Dictionary<IapStoreConfig, string>();
        public List<PayoutDefinitionWrapper> PayoutDefinitions = new List<PayoutDefinitionWrapper>();
    }
}