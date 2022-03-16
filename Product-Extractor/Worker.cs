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
                    if (time < 3) throw new DelayOutOfRangeException();

                    List<Producto> productosApi = await _api.GetProductsApiAsync();
                    List<Producto> productosCache = await _cache.GetAllProductsUsingRedisCache();

                    if (productosCache.Count == 0)
                    {
                        foreach (var item in productosApi)
                        {
                            await _db.SaveProductAsync(item);
                        }
                    }
                    else
                    {
                        foreach (var item in productosApi)
                        {
                            var prodDb = productosCache.Find(x => x.Code.Trim().Equals(item.Code.Trim()) && x.Sku.Trim().Equals(item.Sku.Trim()));
                            if (prodDb != null)
                            {
                                await _db.UpdateProductAsync(item);
                            }
                            else
                            {
                                await _db.SaveProductAsync(item);
                                _logger.LogWarning($"PRODUCTO: {item.Sku.Trim()} NO REGISTRADO EN BASE DE DATOS - PRODUCTO GUARDADO");
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
            _logger.LogWarning("Worker start");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Worker stop");
            await base.StopAsync(cancellationToken);
        }
    }
}
