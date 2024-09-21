using UnityEngine;
using UnityEngine.Purchasing;

namespace LegedaryTools.Mothership.Iap.Store
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Iap/Stores/AppleStoreConfig", fileName = "AppleStoreConfig", order = 0)]
    public class AppleStoreConfig : IapStoreConfig
    {
        public override string Name => MacAppStore.Name;
    }
}