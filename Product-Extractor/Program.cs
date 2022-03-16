using Core.Interfaces;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product_Extractor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    var workerSettings = new WorkerSettings();
                    hostContext.Configuration.Bind(nameof(WorkerSettings), workerSettings);
                    services.AddSingleton(workerSettings);

                    services.AddDbContext<WorkerDbContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration["ConnectionStrings:DefaultConnection"]);
                    });

                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = hostContext.Configuration["ConnectionStrings:Redis"];
                    });

                    services.AddSingleton<IProductoRepository, ProductoRepository>();
                    services.AddSingleton<IApiProductExtractorService, ApiProductExtractorService>();
                    services.AddSingleton<ICacheRedis, CacheRedis>();
                });
    }
}
