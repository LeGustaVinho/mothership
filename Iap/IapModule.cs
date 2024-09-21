using System.Threading.Tasks;
using UnityEngine;

namespace LegedaryTools.Mothership.Iap
{
    public class IapModule : 
#if ODIN_INSPECTOR
        Sirenix.OdinInspector.SerializedScriptableObject
#else
        ScriptableObject
#endif
        , IModule
    {
        public bool Enabled { get; set; }
        
        public Task Initialize()
        {
            throw new System.NotImplementedException();
        }
        
        public void Dispose()
        {
            
        }
    }
}