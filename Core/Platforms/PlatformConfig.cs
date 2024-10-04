using LegendaryTools;
using UnityEngine;

namespace LegedaryTools.Mothership
{
    public abstract class PlatformConfig : UniqueScriptableObject
    {
        public Sprite Icon;
        public abstract bool IsCurrent { get; }
    }
}