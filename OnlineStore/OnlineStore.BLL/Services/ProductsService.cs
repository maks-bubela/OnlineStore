using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;

namespace OnlineStore.BLL.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IMapper _mapper;
        private readonly OnlineStoreContext _ctx;

        private const long ID_NOT_FOUND = 0;

        public ProductsService(OnlineStoreContext ctx, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task<ProductsDTO> GetAvailableProductsByIdAsync(long productsId)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));

            var products = await _ctx.Set<Products>().Where(p => p.Quantity > 0)
                .SingleOrDefaultAsync(x => x.Id == productsId);
            if (products == null) throw new NullReferenceException(nameof(products));

            var productsDTO = _mapper.Map<ProductsDTO>(products);
            return productsDTO;
        }

        public async Task<List<ProductsDTO>> GetAvailableProductsListAsync()
        {
            var productsList = await _ctx.Set<Products>()
                .Where(p => p.Quantity > 0).ToListAsync();
            if (productsList == null) throw new NullReferenceException(nameof(productsList));

            var productsDTO = _mapper.Map<List<ProductsDTO>>(productsList);
            return productsDTO;
        }

        public async Task<bool> ProductStashed(long productsId, long quantity)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));
            if(await IsProductsAvaibleById(productsId, quantity))
            {
                var products = await _ctx.Set<Products>()
                .SingleOrDefaultAsync(x => x.Id == productsId);
                products.Quantity -= quantity;
                _ctx.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> IsProductsAvaibleById(long productsId, long quantity)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));
            var products = await IsProductsExistById(productsId);
            if (products == null) throw new NullReferenceException(nameof(products));
            return products.Quantity >= quantity;
        }

        private async Task<ProductsDTO> IsProductsExistById(long productsId)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));

            var products = await _ctx.Set<Products>()
                .SingleOrDefaultAsync(x => x.Id == productsId);
            if (products == null) throw new NullReferenceException(nameof(products));

            var productsDTO = _mapper.Map<ProductsDTO>(products);
            return productsDTO;
        }
    }
}
