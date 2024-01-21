using OnlineStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Interfaces
{
    public interface IProductsService
    {
        Task<List<ProductsDTO>> GetAvailableProductsListAsync();
        Task<ProductsDTO> GetAvailableProductsByIdAsync(long productsId);
        Task<bool> IsProductsAvaibleById(long productsId, long quantity);
        Task<bool> ProductStashed(long productsId, long quantity);
    }
}
