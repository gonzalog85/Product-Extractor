using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductoRepository
    {
        Task DeleteAsync(Product products);
        Task<List<Product>> GetListAsync();
        Task SaveProductAsync(Product products);
        Task UpdateProductAsync(Product products);
    }
}