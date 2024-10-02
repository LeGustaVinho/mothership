using System;
using System.Threading.Tasks;

namespace LegedaryTools.Mothership
{
    public abstract class InitalizeableScriptableObject :
#if ODIN_INSPECTOR
        Sirenix.OdinInspector.SerializedScriptableObject
#else
        UnityEngine.ScriptableObject
#endif
        , IInitalizeable
    {
#if !ODIN_INSPECTOR
        [SerializeField]
#endif
        protected bool enabled;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (value == enabled) return;
                OnEnableChange(value);
                enabled = value;
            }
        }

#if !ODIN_INSPECTOR
        [SerializeField]
#endif
        protected int timeOut;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.MinValue(1)]
        [Sirenix.OdinInspector.SuffixLabel("seconds")]
#endif
        public int TimeOut
        {
            get => timeOut;
            set => timeOut = value;
        }

        [NonSerialized]
        private bool isInitialized;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        public bool IsInitialized
        {
             get => isInitialized;
             protected set => isInitialized = value;
        }

        public abstract Task Initialize();
        public abstract void OnEnableChange(bool newMode);
        public abstract void Dispose();
    }
}