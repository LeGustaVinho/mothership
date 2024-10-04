using UnityEngine;

namespace LegedaryTools.Mothership
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Platforms/IOSPlatformConfig", fileName = "IOSPlatformConfig", order = 0)]
    public class IOSPlatformConfig : PlatformConfig
    {
        public override bool IsCurrent
        {
            get
            {
#if UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }
    }
}