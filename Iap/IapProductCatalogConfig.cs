using System.Collections.Generic;
using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership.Iap
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Iap/IapProductCatalogConfig", fileName = "IapProductCatalogConfig", order = 0)]
    public class IapProductCatalogConfig : UniqueScriptableObject
    {
        public List<IapProductConfig> Products = new List<IapProductConfig>();
    }
}