using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task SaveProductAsync(Product product)
        {
            var context = GetDbContext();
            context.Add(product);
            await context.SaveChangesAsync();
            _logger.LogInformation($"PRODUCTO: {product.Sku} GUARDADO EN BASE DE DATOS");
        }

        public async Task UpdateProductAsync(Product product)
        {
            var context = GetDbContext();
            var prodDb = context.Products.FirstOrDefault(x => x.Code.Trim().Equals(product.Code.Trim()) && x.Sku.Trim().Equals(product.Sku.Trim()));

            prodDb.Code = product.Code;
            prodDb.Sku = product.Sku;
            prodDb.Stock = product.Stock;
            prodDb.Currency = product.Currency;
            prodDb.Price = product.Price;
            prodDb.Iva = product.Iva;
            prodDb.Ii = product.Ii;

            await context.SaveChangesAsync();
            _logger.LogInformation($"PRODUCTO: {product.Sku} ACTUALIZADO");
        }

        public async Task<List<Product>> GetListAsync()
        {
            var context = GetDbContext();
            return await context.Products.ToListAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            var context = GetDbContext();
            context.Remove(product);
            await context.SaveChangesAsync();
            _logger.LogInformation($"PRODUCTO: {product.Sku} ELIMINADO");
        }
    }
}
