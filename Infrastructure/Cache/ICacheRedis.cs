using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public interface ICacheRedis
    {
        Task<List<Producto>> GetAllProductsUsingRedisCache();
        Task<Producto> GetProductoUsingCacheRedis(string code, string sku);
    }
}