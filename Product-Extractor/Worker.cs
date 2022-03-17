using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.Cache;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Product_Extractor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProductoRepository _db;
        private readonly WorkerSettings _settings;
        private readonly ICacheRedis _cache;
        private readonly IApiProductExtractorService _api;

        public Worker(ILogger<Worker> logger,
            IProductoRepository db,
            WorkerSettings settings,
            ICacheRedis cache, IApiProductExtractorService api)
        {
            _logger = logger;
            _db = db;
            _settings = settings;
            _cache = cache;
            _api = api;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    var time = (double)_settings.DelayMinutes;
                    if (time < 2) throw new DelayOutOfRangeException();

                    List<Product> productosApi = await _api.GetProductsApiAsync();
                    //List<Product> productosApi = new List<Product>(); // Lista vacia prueba

                    List<Product> productosCache = await _cache.GetAllProductsUsingRedisCache();
                    //List<Products> productosCache = new List<Products>(); // Lista vacia prueba

                    _logger.LogWarning($" Api Count: {productosApi.Count} - Cache Count: {productosCache.Count}");

                    if (productosCache.Count > productosApi.Count)
                    {
                        foreach (var prodCache in productosCache)
                        {
                            var prodApi = productosApi.Find(x => x.Code.Trim().Equals(prodCache.Code.Trim()) && x.Sku.Trim().Equals(prodCache.Sku.Trim()));
                            if (prodApi != null)
                            {
                                await _db.UpdateProductAsync(prodApi);

                            }
                            else
                            {
                                await _db.DeleteAsync(prodCache);
                            }
                        }
                    }
                    else
                    {
                        foreach (var prodApi in productosApi)
                        {
                            var prodDb = productosCache.Find(x => x.Code.Trim().Equals(prodApi.Code.Trim()) && x.Sku.Trim().Equals(prodApi.Sku.Trim()));
                            if (prodDb != null)
                            {
                                await _db.UpdateProductAsync(prodApi);
                            }
                            else
                            {
                                await _db.SaveProductAsync(prodApi);
                            }
                        }
                    }

                    _logger.LogInformation("TAREA FINALIZADA - BASE DE DATOS ACTUALIZADA");
                    await Task.Delay(TimeSpan.FromMinutes(time), stoppingToken);
                }
            }
            catch (DelayOutOfRangeException e)
            {
                _logger.LogError(e.Message);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker start");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stop");
            await base.StopAsync(cancellationToken);
        }
    }
}
