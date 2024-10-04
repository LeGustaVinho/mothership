using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LegedaryTools.Mothership.Ads.AppLovinMax
{
    public class AppLovinMaxRewardedAds : AdsRewarded
    {
        private Dictionary<string, AdsConfig> adsConfigByIdOfPlatform;

        public override event Action<AdsConfig> OnLoad;
        public override event Action<AdsConfig, AdResponseErrorCode, string> OnLoadFail;
        public override event Action<AdsConfig> OnShow;
        public override event Action<AdsConfig, AdResponseErrorCode, string> OnShowFail;
        public override event Action<AdsConfig> OnHide;
        public override event Action<AdsConfig> OnClick;
        public override event Action<AdsConfig> OnRewardReceived;

        public override void Initialize(Dictionary<string, AdsConfig> adsConfigByIdOfPlatform)
        {
            this.adsConfigByIdOfPlatform = adsConfigByIdOfPlatform;
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdHiddenEvent;
        }
        
        public override void Dispose()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnAdHiddenEvent;
        }

        public override void Load(AdsConfig config)
        {
            string adUnitIdentifier = config.IdOfCurrentPlatform;
            if (string.IsNullOrEmpty(adUnitIdentifier))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(Load)}] AdsConfig {config.name} IdOfCurrentPlatform is empty.");
                return;
            }

            if (config.Type != Type)
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(Load)}] AdsConfig {config.name} is type {config.Type} but {nameof(AppLovinMaxRewardedAds)} expects {Type}.");
                return;
            }
            
            MaxSdk.LoadRewardedAd(adUnitIdentifier);
        }

        public override bool IsReady(AdsConfig config)
        {
            string adUnitIdentifier = config.IdOfCurrentPlatform;
            if (string.IsNullOrEmpty(adUnitIdentifier))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(IsReady)}] AdsConfig {config.name} IdOfCurrentPlatform is empty.");
                return false;
            }

            if (config.Type != Type)
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(IsReady)}] AdsConfig {config.name} is type {config.Type} but {nameof(AppLovinMaxRewardedAds)} expects {Type}.");
                return false;
            }
            
            return MaxSdk.IsRewardedAdReady(adUnitIdentifier);
        }
        
        public override async void Show(AdsConfig config, string placement = null, string customData = null)
        {
            string adUnitIdentifier = config.IdOfCurrentPlatform;
            if (string.IsNullOrEmpty(adUnitIdentifier))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(Show)}] AdsConfig {config.name} IdOfCurrentPlatform is empty.");
                return;
            }

            if (config.Type != Type)
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(Show)}] AdsConfig {config.name} is type {config.Type} but {nameof(AppLovinMaxRewardedAds)} expects {Type}.");
                return;
            }

            if (IsReady(config))
            {
                Show(config, placement, customData);
                return;
            }
            
            Task timeoutTask = Task.Delay(config.WaitLoadTimeOut);
            while (!IsReady(config))
            {
                Task delayTask = Task.Delay(100);
                Task completedTask = await Task.WhenAny(delayTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    return;
                }
            }
            
            if (IsReady(config))
                Show(config, placement, customData);
        }
        
        private void OnAdLoadedEvent(string adUnitIdentifier, MaxSdkBase.AdInfo adInfo)
        {
            if (!adsConfigByIdOfPlatform.TryGetValue(adUnitIdentifier, out AdsConfig config))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(OnAdLoadedEvent)}] adUnitIdentifier {adUnitIdentifier} was not found in adsConfigByIdOfPlatform.");
                return;
            }
            OnLoad?.Invoke(config);
        }

        private void OnAdLoadFailedEvent(string adUnitIdentifier, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (!adsConfigByIdOfPlatform.TryGetValue(adUnitIdentifier, out AdsConfig config))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(OnAdLoadFailedEvent)}] adUnitIdentifier {adUnitIdentifier} was not found in adsConfigByIdOfPlatform.");
                return;
            }
            OnLoadFail?.Invoke(config, errorInfo.Code.ToAdResponseErrorCode(), errorInfo.Message);
        }

        private void OnAdDisplayedEvent(string adUnitIdentifier, MaxSdkBase.AdInfo adInfo)
        {
            if (!adsConfigByIdOfPlatform.TryGetValue(adUnitIdentifier, out AdsConfig config))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(OnAdDisplayedEvent)}] adUnitIdentifier {adUnitIdentifier} was not found in adsConfigByIdOfPlatform.");
                return;
            }
            OnShow?.Invoke(config);
        }

        private void OnAdDisplayFailedEvent(string adUnitIdentifier, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            if (!adsConfigByIdOfPlatform.TryGetValue(adUnitIdentifier, out AdsConfig config))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(OnAdDisplayFailedEvent)}] adUnitIdentifier {adUnitIdentifier} was not found in adsConfigByIdOfPlatform.");
                return;
            }
            OnShowFail?.Invoke(config, errorInfo.Code.ToAdResponseErrorCode(), errorInfo.Message);
        }

        private void OnAdClickedEvent(string adUnitIdentifier, MaxSdkBase.AdInfo adInfo)
        {
            if (!adsConfigByIdOfPlatform.TryGetValue(adUnitIdentifier, out AdsConfig config))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(OnAdClickedEvent)}] adUnitIdentifier {adUnitIdentifier} was not found in adsConfigByIdOfPlatform.");
                return;
            }
            OnClick?.Invoke(config);
        }

        private void OnAdReceivedRewardEvent(string adUnitIdentifier, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            if (!adsConfigByIdOfPlatform.TryGetValue(adUnitIdentifier, out AdsConfig config))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(OnAdReceivedRewardEvent)}] adUnitIdentifier {adUnitIdentifier} was not found in adsConfigByIdOfPlatform.");
                return;
            }
            OnRewardReceived?.Invoke(config);
        }

        private void OnAdHiddenEvent(string adUnitIdentifier, MaxSdkBase.AdInfo adInfo)
        {
            if (!adsConfigByIdOfPlatform.TryGetValue(adUnitIdentifier, out AdsConfig config))
            {
                if (Mothership.LogLevel.HasFlag(MothershipLogLevel.Error))
                    Debug.LogError($"[{nameof(AppLovinMaxRewardedAds)}:{nameof(OnAdHiddenEvent)}] adUnitIdentifier {adUnitIdentifier} was not found in adsConfigByIdOfPlatform.");
                return;
            }
            OnHide?.Invoke(config);
        }
    }
}