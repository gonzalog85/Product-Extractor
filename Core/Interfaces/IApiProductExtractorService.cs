using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IApiProductExtractorService
    {
        Task<List<Products>> GetProductsApiAsync();
    }
}