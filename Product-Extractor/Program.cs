using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product_Extractor.Cache;
using Product_Extractor.Services;
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

                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = hostContext.Configuration["ConnectionStrings:Redis"];
                    });

                    services.AddTransient<IDbService, DbService>();
                    services.AddTransient<IApiProductExtractorService, ApiProductExtractorService>();
                    services.AddSingleton<ICacheRedis, CacheRedis>();
                });
    }
}
