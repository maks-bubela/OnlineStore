using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;
using System.Linq.Expressions;

namespace OnlineStore.BLL.Services
{
    public class ProductsService : IProductsService
    {
        #region Services
        private readonly IMapper _mapper;
        private readonly IGenericRepository _repository;
        #endregion

        #region Consts
        private const long ID_NOT_FOUND = 0;
        #endregion
        #region Predicates
        private Expression<Func<Products, bool>> GetOrderByIdPredicate(long productsId)
        {
            Expression<Func<Products, bool>> predicate = x => x.Id == productsId && x.Quantity > 0;
            return predicate;
        }
        private Expression<Func<Products, bool>> GetOrderPredicate()
        {
            Expression<Func<Products, bool>> predicate = x => x.Quantity > 0;
            return predicate;
        }
        private Expression<Func<Products, bool>> IsProductsExistByIdPredicate(long productsId)
        {
            Expression<Func<Products, bool>> predicate = x => x.Id == productsId;
            return predicate;
        }
        #endregion

        public ProductsService(IMapper mapper, IGenericRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); ;
        }

        public async Task<ProductsDTO> GetAvailableProductsByIdAsync(long productsId)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));
            var predicate = GetOrderByIdPredicate(productsId);
            var products = await _repository.GetAsync(predicate) ?? throw new NotFoundInDatabaseException();

            var productsDTO = _mapper.Map<ProductsDTO>(products);
            return productsDTO;
        }

        public async Task<List<ProductsDTO>> GetAvailableProductsListAsync()
        {
            var predicate = GetOrderPredicate();
            var productsList = await _repository.ListAsync(predicate) ?? throw new NotFoundInDatabaseException();

            var productsDTO = _mapper.Map<List<ProductsDTO>>(productsList);
            return productsDTO;
        }

        public async Task<bool> ProductStashed(long productsId, long quantity)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));
            if(await IsProductsAvaibleById(productsId, quantity))
            {
                var predicate = GetOrderByIdPredicate(productsId);
                var products = await _repository.GetAsync(predicate) ?? throw new NotFoundInDatabaseException();
                products.Quantity -= quantity;
                await _repository.UpdateAsync(products);
                return true;
            }
            return false;
        }

        public async Task<bool> IsProductsAvaibleById(long productsId, long quantity)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));
            if (!await IsProductsExistById(productsId))
                return false;
            var predicate = GetOrderByIdPredicate(productsId);

            var product = await _repository.GetAsync(predicate);
            return product.Quantity >= quantity;
        }

        public async Task<bool> IsProductsExistById(long productsId)
        {
            if (productsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(productsId));
            var predicate = IsProductsExistByIdPredicate(productsId);
            return await _repository.IsExistAsync(predicate);
        }
    }
}
