using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Products>> GetListAsync();
        Task SaveProductAsync(Products producto);
        Task UpdateProductAsync(Products producto);
    }
}