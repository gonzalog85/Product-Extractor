using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public class CacheRedis : ICacheRedis
    {
        private readonly IProductoRepository context;
        private readonly IDistributedCache distributedCache;

        public CacheRedis(IProductoRepository context, IDistributedCache distributedCache)
        {
            this.context = context;
            this.distributedCache = distributedCache;
        }

        public async Task<List<Products>> GetAllProductsUsingRedisCache()
        {
            var cacheKey = "productList";
            string serializedProductList;
            var productList = new List<Products>();
            var redisProductList = await distributedCache.GetAsync(cacheKey);
            if (redisProductList != null)
            {
                serializedProductList = Encoding.UTF8.GetString(redisProductList);
                productList = JsonConvert.DeserializeObject<List<Products>>(serializedProductList);
            }
            else
            {
                productList = await context.GetListAsync();

                serializedProductList = JsonConvert.SerializeObject(productList);
                redisProductList = Encoding.UTF8.GetBytes(serializedProductList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(30))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                
                await distributedCache.SetAsync(cacheKey, redisProductList, options);
            }
            return productList;
        }

        public async Task<Products> GetProductoUsingCacheRedis(string code, string sku)
        {
            var productList = await context.GetListAsync();
            return productList.Find(x => x.Code.Trim().Equals(code) && x.Sku.Trim().Equals(sku));
        }
    }
}
