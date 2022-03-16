using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public interface ICacheRedis
    {
        Task<List<Products>> GetAllProductsUsingRedisCache();
        Task<Products> GetProductoUsingCacheRedis(string code, string sku);
    }
}