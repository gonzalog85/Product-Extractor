using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetListAsync();
        Task SaveProductAsync(Producto producto);
        Task UpdateProductAsync(Producto producto);
    }
}