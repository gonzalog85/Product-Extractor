using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public interface ICacheRedis
    {
        Task<List<Product>> GetAllProductsUsingRedisCache();
        Task<Product> GetProductoUsingCacheRedis(string code, string sku);
    }
}