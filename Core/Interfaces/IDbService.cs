using Product_Extractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product_Extractor.Services
{
    public interface IDbService
    {
        Task<List<Producto>> GetListAsync();
        Task SaveProductAsync(Producto producto);
        Task UpdateProductAsync(Producto producto);
    }
}