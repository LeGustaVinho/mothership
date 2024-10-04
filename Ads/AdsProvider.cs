using System;
using System.Collections.Generic;

namespace LegedaryTools.Mothership.Ads
{
    public enum AdsType
    {
        //Fixed
        Banner,
        MRECs,
        
        //Dynamic
        Interstitials,
        AppOpenAds,
        Rewarded,
        RewardedInterstitial
    }
    
    public abstract class AdsProvider : BaseProvider
    {
        public AdsConfig[] Configs;
        public abstract Dictionary<AdsType, AdsDynamic> DynamicAdSources { get; }
        public abstract Dictionary<AdsType, AdsFixed> FixedAdSources { get; }
        public abstract Dictionary<string, AdsConfig> AdsConfigByIdOfPlatform { get; }
        
        public abstract bool UserConsent { get; set; }
        public abstract bool IsAgeRestrictedUser { get; set; }
        public abstract bool DoNotSell { get; set; }
        public abstract bool Mute { get; set; }
        public abstract bool VerboseLogging { get; set; }
        public abstract bool CreativeDebuggerEnabled { get; set; }

        public abstract void ShowMediationDebugger();
        public abstract void ShowCreativeDebugger();
        public abstract void SetTestDeviceAdvertisingIdentifiers(string[] advertisingIdentifiers);
    }
}