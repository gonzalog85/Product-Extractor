using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Product_Extractor.Cache;
using Product_Extractor.Exceptions;
using Product_Extractor.Models;
using Product_Extractor.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Product_Extractor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDbService _db;
        private readonly WorkerSettings _settings;
        private readonly ICacheRedis cache;
        private readonly IApiProductExtractorService api;

        public Worker(ILogger<Worker> logger,
            IDbService db,
            WorkerSettings settings,
            ICacheRedis cache, IApiProductExtractorService api)
        {
            _logger = logger;
            _db = db;
            _settings = settings;
            this.cache = cache;
            this.api = api;
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

                    List<Producto> productosApi = await api.GetProductsApiAsync();
                    List<Producto> productosCache = cache.GetAllProductsUsingRedisCache().Result;

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
