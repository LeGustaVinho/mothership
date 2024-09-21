using UnityEngine;

namespace LegedaryTools.Mothership
{
    [CreateAssetMenu(fileName = "MothershipConfig", menuName = "Tools/Mothership/MothershipConfig")]
    public class MothershipConfig : ScriptableObject
    {
        public MothershipLogLevel LogLevel;
        public MothershipModule[] Modules;
    }
}