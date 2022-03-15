using Product_Extractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product_Extractor.Services
{
    public interface IApiProductExtractorService
    {
        Task<List<Producto>> GetProductsApiAsync();
    }
}