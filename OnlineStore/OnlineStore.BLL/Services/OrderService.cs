using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;

namespace OnlineStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly IStripeService _stripeService;
        private readonly ICustomerService _customerService;
        private readonly OnlineStoreContext _ctx;

        private const long ID_NOT_FOUND = 0;

        public OrderService(OnlineStoreContext ctx, IMapper mapper, ICustomerService customerService, 
            IStripeService stripeService, IProductsService productsService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _stripeService = stripeService ?? throw new ArgumentNullException(nameof(stripeService));
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
        }

        public async Task<long> CreateOrderAsync(OrderDTO orderDTO)
        {
            if (orderDTO == null) throw new ArgumentNullException(nameof(orderDTO));
            if (!await _customerService.CustomerExistsAsync(orderDTO.CustomerId)) throw new NotFoundInDatabaseException();
            if (!await _productsService.IsProductsAvaibleById(orderDTO.ProductsId, orderDTO.QuantityProducts)) throw new NotFoundInDatabaseException();

            var order = _mapper.Map<Order>(orderDTO);

            _ctx.Orders.Add(order);
            await _ctx.SaveChangesAsync();
            return order.Id;
        }

        public async Task<string> OrderProccessingCard(long orderId)
        {
            if (orderId == ID_NOT_FOUND) throw new NotFoundInDatabaseException();
            var order = await _ctx.Set<Order>().Where(x => x.Id == orderId).Include(x => x.Products)
                .SingleOrDefaultAsync();

            var productsDTO = _mapper.Map<ProductsDTO>(order.Products);
            var sessionId = await _stripeService.CreateCheckoutSessionAsync(productsDTO);
            if(sessionId == null) throw new Exception(nameof(sessionId));
            order.SessionId = sessionId;
            if(!await _productsService.ProductStashed(order.ProductsId, order.QuantityProducts)) 
                throw new Exception(nameof(_productsService));
            await _ctx.SaveChangesAsync();

            return sessionId;
        }

        public async Task<bool> OrderProccessingCash(long orderId)
        {
            if (orderId == ID_NOT_FOUND) throw new NotFoundInDatabaseException();
            var order = await _ctx.Set<Order>().Where(x => x.Id == orderId).Include(x => x.Products)
                .SingleOrDefaultAsync();

            var productsDTO = _mapper.Map<ProductsDTO>(order.Products);
            if (!await _productsService.ProductStashed(order.ProductsId, order.QuantityProducts))
                throw new Exception(nameof(_productsService));
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
