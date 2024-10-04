using UnityEngine;

namespace LegedaryTools.Mothership
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Platforms/UnityEditorPlatformConfig", fileName = "UnityEditorPlatformConfig", order = 0)]
    public class UnityEditorPlatformConfig : PlatformConfig
    {
        public override bool IsCurrent
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
    }
}