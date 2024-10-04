using System;
using System.Collections.Generic;
using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership.Ads
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Ads/AdsConfig", fileName = "AdsConfig", order = 0)]
    public class AdsConfig : UniqueScriptableObject
    {
        public AdsType Type;
        public bool Preload;
        public bool AutoRequestAfterShow;
        public int WaitLoadTimeOut;

        public bool IsFixed => Type == AdsType.Banner || Type == AdsType.MRECs;

        public bool IsDynamic => Type == AdsType.RewardedInterstitial || Type == AdsType.Interstitials || Type == AdsType.Rewarded ||
                                 Type == AdsType.AppOpenAds;

        public string IdOfCurrentPlatform
        {
            get
            {
#if ODIN_INSPECTOR
                foreach (KeyValuePair<PlatformConfig, string> pair in Ids)
#else
                foreach (KeyValuePair<PlatformConfig, string> pair in Ids.Dictionary)
#endif
                {
                    if (pair.Key.IsCurrent) return pair.Value;
                }
                return string.Empty;
            }
        }
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
        public Dictionary<PlatformConfig, string> Ids = new Dictionary<PlatformConfig, string>();
#else
        [Serializable]
        public class PlatformConfigWithIdPair : IKeyValuePair<PlatformConfig, string>
        {
            public string Id;
            public PlatformConfig Config;

            public PlatformConfig Key
            {
                get => Config;
                set => Config = value;
            }

            public string Value
            {
                get => Id;
                set => Id = value;
            }
        }
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.DrawWithUnity]
#endif
        public MappedList<PlatformConfigWithIdPair, PlatformConfig, string> Ids =
            new MappedList<PlatformConfigWithIdPair, PlatformConfig, string>();
#endif
    }
}