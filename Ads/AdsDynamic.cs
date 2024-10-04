using System;
using System.Collections.Generic;

namespace LegedaryTools.Mothership.Ads
{
    public enum AdResponseErrorCode
    {
        Unspecified,
        NoFill,
        AdLoadFailed,
        AdDisplayFailed,
        NetworkError,
        NetworkTimeout,
        NoNetwork,
        FullscreenAdAlreadyShowing,
        FullscreenAdNotReady,
        FullscreenAdInvalidViewController,
        FullscreenAdAlreadyLoading,
        FullscreenAdLoadWhileShowing,
        DontKeepActivitiesEnabled,
        InvalidAdUnitId
    }

    /**
     * This enum contains possible states of an ad in the waterfall the adapter response info could represent.
     */
    public enum MaxAdLoadState
    {
        /// <summary>
        /// The AppLovin Max SDK did not attempt to load an ad from this network in the waterfall because an ad higher
        /// in the waterfall loaded successfully.
        /// </summary>
        AdLoadNotAttempted,

        /// <summary>
        /// An ad successfully loaded from this network.
        /// </summary>
        AdLoaded,

        /// <summary>
        /// An ad failed to load from this network.
        /// </summary>
        FailedToLoad
    }
    
    public abstract class AdsDynamic
    {
        public abstract event Action<AdsConfig> OnLoad;
        public abstract event Action<AdsConfig, AdResponseErrorCode, string> OnLoadFail;
        public abstract event Action<AdsConfig> OnShow;
        public abstract event Action<AdsConfig, AdResponseErrorCode, string> OnShowFail;
        public abstract event Action<AdsConfig> OnHide;
        public abstract event Action<AdsConfig> OnClick;
        public abstract event Action<AdsConfig> OnRewardReceived;
        public abstract AdsType Type { get; }
        public abstract void Initialize(Dictionary<string, AdsConfig> AdsConfigByIdOfPlatform);
        public abstract void Dispose();
        public abstract void Load(AdsConfig config);
        public abstract bool IsReady(AdsConfig config);
        public abstract void Show(AdsConfig config, string placement = null, string customData = null);
    }
}