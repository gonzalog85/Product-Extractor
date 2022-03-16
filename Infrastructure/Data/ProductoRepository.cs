using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ILogger<ProductoRepository> _logger;
        private readonly IServiceScopeFactory _factory;

        public ProductoRepository(ILogger<ProductoRepository> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        private WorkerDbContext GetDbContext()
        {
            var scope = _factory.CreateScope();
            return scope.ServiceProvider.GetService<WorkerDbContext>();
        }

        public async Task SaveProductAsync(Producto producto)
        {
            var context = GetDbContext();
            context.Add(producto);
            await context.SaveChangesAsync();
            _logger.LogInformation($"PRODUCTO: {producto.Sku} GUARDADO EN BASE DE DATOS");
        }

        public async Task UpdateProductAsync(Producto producto)
        {
            var context = GetDbContext();
            var prodDb = context.Productos.FirstOrDefault(x => x.Code.Trim().Equals(producto.Code.Trim()) && x.Sku.Trim().Equals(producto.Sku.Trim()));
            await context.SaveChangesAsync();
            _logger.LogInformation($"PRODUCTO: {producto.Sku} ACTUALIZADO");
        }

        public async Task<List<Producto>> GetListAsync()
        {
            var context = GetDbContext();
            List<Producto> productos = new List<Producto>();
            var productosDb = await context.Productos.ToListAsync();

            foreach (var item in productosDb)
            {
                productos.Add(item);
            }
            return productos;
        }
    }
}
