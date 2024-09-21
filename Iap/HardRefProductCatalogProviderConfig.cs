using System.Threading.Tasks;
using LegedaryTools.Mothership;
using LegedaryTools.Mothership.Iap;
using UnityEngine;

namespace mothership.Iap
{
    [CreateAssetMenu(menuName = "Tools/Mothership/Iap/HardRefProductCatalogProviderConfig", fileName = "HardRefProductCatalogProviderConfig", order = 0)]
    public class HardRefProductCatalogProviderConfig : IapProductCatalogProviderConfig
    {
        public IapProductCatalogConfig ProductCatalogConfig;
        
        public override async Task<IapProductCatalogConfig> GetProductCatalog()
        {
            await Task.Yield();
            return ProductCatalogConfig;
        }
    }
}