#if ENABLE_MAX_ADS
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LegedaryTools.Mothership.Ads.AppLovinMax
{
    public class AppLovinMaxAdsProvider : AdsProvider
    {
        public override Dictionary<AdsType, AdsDynamic> DynamicAdSources { get; } =
            new Dictionary<AdsType, AdsDynamic>();

        public override Dictionary<AdsType, AdsFixed> FixedAdSources { get; } = new Dictionary<AdsType, AdsFixed>();

        public override Dictionary<string, AdsConfig> AdsConfigByIdOfPlatform { get; } =
            new Dictionary<string, AdsConfig>();
        
        public override bool UserConsent { get; set; }
        public override bool IsAgeRestrictedUser { get; set; }
        public override bool DoNotSell { get; set; }
        public override bool Mute { get; set; }
        public override bool VerboseLogging { get; set; }
        public override bool CreativeDebuggerEnabled { get; set; }
        
        public override async Task Initialize()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            void OnSdkInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration)
            {
                MaxSdkCallbacks.OnSdkInitializedEvent -= OnSdkInitialized;
                tcs.SetResult(true);
            }

#if MAX_DEBUG_LOGGING
            MaxSdk.SetVerboseLogging(true);
#endif
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
            MaxSdk.InitializeSdk();
            await tcs.Task;
            
            DynamicAdSources.Add(AdsType.Rewarded, new AppLovinMaxRewardedAds());

            foreach (AdsConfig adsConfig in Configs)
            {
                if (!AdsConfigByIdOfPlatform.TryAdd(adsConfig.IdOfCurrentPlatform, adsConfig))
                {
                    if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Warning))
                        Debug.LogWarning($"[{nameof(AppLovinMaxAdsProvider)}:{nameof(Initialize)}] AdUnitId {adsConfig.IdOfCurrentPlatform} of AdsConfig {adsConfig.name} is duplicated with AdsConfig {AdsConfigByIdOfPlatform[adsConfig.IdOfCurrentPlatform].name}.");
                    continue;
                }

                if (adsConfig.Preload)
                {
                    if (adsConfig.IsDynamic && DynamicAdSources.TryGetValue(adsConfig.Type, out AdsDynamic adsDynamic))
                        adsDynamic.Load(adsConfig);
                    
                    if (adsConfig.IsFixed && FixedAdSources.TryGetValue(adsConfig.Type, out AdsFixed adsFixed))
                    {
                        //TODO:
                    }
                }
            }

            foreach (KeyValuePair<AdsType, AdsDynamic> pair in DynamicAdSources)
            {
                pair.Value.Initialize(AdsConfigByIdOfPlatform);
            }
            
            IsInitialized = true;
        }

        public override void Dispose()
        {
        }
        
        public override void ShowMediationDebugger()
        {
        }

        public override void ShowCreativeDebugger()
        {
        }

        public override void SetTestDeviceAdvertisingIdentifiers(string[] advertisingIdentifiers)
        {
        }
    }
}
#endif