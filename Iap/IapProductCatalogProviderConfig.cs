using System.Threading.Tasks;
using LegedaryTools.Mothership.Iap;

namespace LegedaryTools.Mothership
{
    public abstract class IapProductCatalogProviderConfig : 
#if ODIN_INSPECTOR
    Sirenix.OdinInspector.SerializedScriptableObject
#else
        ScriptableObject
#endif
    {
        public abstract Task<IapProductCatalogConfig> GetProductCatalog();
    }
}