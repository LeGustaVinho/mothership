using System.Threading.Tasks;
using UnityEngine;

namespace LegedaryTools.Mothership
{
    public abstract class MothershipModule : ScriptableObject, IModule
    {
        protected bool enabled;
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (value != enabled)
                {
                    OnEnableChange(value);
                    enabled = value;
                }
            }
        }
        
        public abstract Task Initialize();

        public abstract void Dispose();

        public abstract void OnEnableChange(bool newMode);
    }
}