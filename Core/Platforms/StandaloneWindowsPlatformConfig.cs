using UnityEngine;

namespace LegedaryTools.Mothership
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Platforms/StandaloneWindowsPlatformConfig", fileName = "StandaloneWindowsPlatformConfig", order = 0)]
    public class StandaloneWindowsPlatformConfig : PlatformConfig
    {
        public override bool IsCurrent
        {
            get
            {
#if UNITY_STANDALONE_WIN
                return true;
#else
                return false;
#endif
            }
        }
    }
}