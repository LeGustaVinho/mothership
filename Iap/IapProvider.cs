using System;
using System.Threading.Tasks;

namespace LegedaryTools.Mothership.Iap
{
    public abstract class IapProvider :
#if ODIN_INSPECTOR
        Sirenix.OdinInspector.SerializedScriptableObject
#else
        ScriptableObject
#endif
    , IDisposable
    {
        public IapProductCatalogProviderConfig ProductCatalogProvider;
        
        public bool IsInitialized { get; protected set; }
        
        public abstract Task Initialize();
        public abstract void Dispose();
    }
}