using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Product_Extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product_Extractor.Services
{
    public class DbService : IDbService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<DbService> _logger;

        public DbService(IConfiguration config, ILogger<DbService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SaveProductAsync(Producto producto)
        {
            using (var context = new ProductosApiContext(_config.GetConnectionString("DefaultConnection")))
            {
                context.Add(producto);
                await context.SaveChangesAsync();
                _logger.LogInformation($"PRODUCTO: {producto.Sku} GUARDADO EN BASE DE DATOS");
            }
        }

        public async Task UpdateProductAsync(Producto producto)
        {
            using (var context = new ProductosApiContext(_config.GetConnectionString("DefaultConnection")))
            {
                var prodDb = context.Productos.FirstOrDefault(x => x.Code.Trim().Equals(producto.Code.Trim()) && x.Sku.Trim().Equals(producto.Sku.Trim()));
                await context.SaveChangesAsync();
                _logger.LogInformation($"PRODUCTO: {producto.Sku} ACTUALIZADO");
            }
        }

        public async Task<List<Producto>> GetListAsync()
        {
            List<Producto> productos = new List<Producto>();
            using (var context = new ProductosApiContext(_config.GetConnectionString("DefaultConnection")))
            {
                var productosDb = await context.Productos.ToListAsync();

                foreach (var item in productosDb)
                {
                    productos.Add(item);
                }
            }
            return productos;
        }
    }
}
