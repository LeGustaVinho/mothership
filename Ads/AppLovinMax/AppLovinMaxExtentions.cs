using System;

namespace LegedaryTools.Mothership.Ads.AppLovinMax
{
    public static class AppLovinMaxExtentions
    {
        public static AdResponseErrorCode ToAdResponseErrorCode(this MaxSdkBase.ErrorCode errorCode)
        {
            if (Enum.TryParse<AdResponseErrorCode>(errorCode.ToString(), out var result))
            {
                return result;
            }
            return AdResponseErrorCode.Unspecified;
        }
        
        public static MaxSdkBase.ErrorCode ToErrorCode(this AdResponseErrorCode adResponseErrorCode)
        {
            if (Enum.TryParse<MaxSdkBase.ErrorCode>(adResponseErrorCode.ToString(), out var result))
            {
                return result;
            }
            return MaxSdkBase.ErrorCode.Unspecified;
        }
    }
}