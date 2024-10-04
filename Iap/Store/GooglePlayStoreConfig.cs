using UnityEngine;

namespace LegedaryTools.Mothership.Iap.Store
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Iap/Stores/GooglePlayStoreConfig", fileName = "GooglePlayStoreConfig", order = 0)]
    public class GooglePlayStoreConfig : IapStoreConfig
    {
        public override string Name => "GooglePlay";
    }
}