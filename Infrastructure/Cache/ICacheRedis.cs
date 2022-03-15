using Product_Extractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product_Extractor.Cache
{
    public interface ICacheRedis
    {
        Task<List<Producto>> GetAllProductsUsingRedisCache();
        Task<Producto> GetProductoUsingCacheRedis(string code, string sku);
    }
}