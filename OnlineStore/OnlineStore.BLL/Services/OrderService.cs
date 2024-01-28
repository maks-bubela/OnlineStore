using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;
using System;
using System.Linq.Expressions;

namespace OnlineStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        #region Services
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly IStripeService _stripeService;
        private readonly ICustomerService _customerService;
        private readonly IGenericRepository _repository;
        #endregion

        #region Const
        private const long ID_NOT_FOUND = 0;
        #endregion

        #region Predicates
        private Expression<Func<Order, bool>> GetOrderByIdPredicate(long orderId)
        {
            Expression<Func<Order, bool>> predicate = x => x.Id == orderId;
            return predicate;
        }
        #endregion

        public OrderService(IMapper mapper, ICustomerService customerService,
            IStripeService stripeService, IProductsService productsService, IGenericRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _stripeService = stripeService ?? throw new ArgumentNullException(nameof(stripeService));
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<long> CreateOrderAsync(OrderDTO orderDTO)
        {
            if (orderDTO == null) throw new ArgumentNullException(nameof(orderDTO));
            if (!await _customerService.CustomerExistsAsync(orderDTO.CustomerId)) throw new NotFoundInDatabaseException();
            if (!await _productsService.IsProductsAvaibleById(orderDTO.ProductsId, orderDTO.QuantityProducts)) throw new NotFoundInDatabaseException();

            var order = _mapper.Map<Order>(orderDTO);

            await _repository.AddAsync(order);
            return order.Id;
        }

        public async Task<string> OrderProccessingCard(long orderId)
        {
            if (orderId == ID_NOT_FOUND) throw new NotFoundInDatabaseException();
            var predicate = GetOrderByIdPredicate(orderId);
            var order = await _repository.
                GetAsync(predicate, x => x.Products) ?? throw new NotFoundInDatabaseException();

            var productsDTO = _mapper.Map<ProductsDTO>(order.Products);
            var sessionId = await _stripeService.CreateCheckoutSessionAsync(productsDTO);
            if(sessionId == null) throw new Exception(nameof(sessionId));
            order.SessionId = sessionId;
            if(!await _productsService.ProductStashed(order.ProductsId, order.QuantityProducts)) 
                throw new Exception(nameof(_productsService));
            await _repository.UpdateAsync(order);

            return sessionId;
        }

        public async Task<bool> OrderProccessingCash(long orderId)
        {
            if (orderId == ID_NOT_FOUND) throw new NotFoundInDatabaseException();
            var predicate = GetOrderByIdPredicate(orderId);
            var order = await _repository.
                GetAsync(predicate, x => x.Products) ?? throw new NotFoundInDatabaseException();

            var productsDTO = _mapper.Map<ProductsDTO>(order.Products);
            if (!await _productsService.ProductStashed(order.ProductsId, order.QuantityProducts))
                throw new Exception(nameof(_productsService));
            return true;
        }
    }
}
