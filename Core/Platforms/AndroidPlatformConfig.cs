using UnityEngine;

namespace LegedaryTools.Mothership
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Platforms/AndroidPlatformConfig", fileName = "AndroidPlatformConfig", order = 0)]
    public class AndroidPlatformConfig : PlatformConfig
    {
        public override bool IsCurrent
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }
    }
}