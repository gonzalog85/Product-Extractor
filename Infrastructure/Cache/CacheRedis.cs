﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Product_Extractor.Models;
using Product_Extractor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Extractor.Cache
{
    public class CacheRedis : ICacheRedis
    {
        private readonly IDbService context;
        private readonly IDistributedCache distributedCache;

        public CacheRedis(IDbService context, IDistributedCache distributedCache)
        {
            this.context = context;
            this.distributedCache = distributedCache;
        }

        public async Task<List<Producto>> GetAllProductsUsingRedisCache()
        {
            var cacheKey = "productList";
            string serializedProductList;
            var productList = new List<Producto>();
            var redisProductList = await distributedCache.GetAsync(cacheKey);
            if (redisProductList != null)
            {
                serializedProductList = Encoding.UTF8.GetString(redisProductList);
                productList = JsonConvert.DeserializeObject<List<Producto>>(serializedProductList);
            }
            else
            {
                productList = await context.GetListAsync();

                serializedProductList = JsonConvert.SerializeObject(productList);
                redisProductList = Encoding.UTF8.GetBytes(serializedProductList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(30))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisProductList, options);
            }
            return productList;
        }

        public async Task<Producto> GetProductoUsingCacheRedis(string code, string sku)
        {
            var productList = await context.GetListAsync();
            return productList.Find(x => x.Code.Trim().Equals(code) && x.Sku.Trim().Equals(sku));
        }
    }
}