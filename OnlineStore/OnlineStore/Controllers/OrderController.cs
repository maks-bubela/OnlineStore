using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Enums;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        #region Constants
        private const long ID_NOT_FOUND = 0;
        private const string ProductsSuccesPayment = "Products was sold";
        private const string ProductsBadPayment = "Products bad payment";
        #endregion

        #region Services
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IStripeService _stripeService;
        #endregion

        public OrderController(IOrderService orderService, ICustomerService customerService, 
            IMapper mapper, IStripeService stripeService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(IOrderService));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(ICustomerService));
            _stripeService = stripeService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
        }

        /// <summary>
        /// Create order.
        /// </summary>
        /// <returns></returns>
        /// <param name="orderCreateModel">Order create model</param>
        /// <response code="200"> Order was created </response>> 
        /// <response code="409"> Order create was faild </response>
        /// <response code="500"> Something is wrong on server </response>>
        [Authorize, HttpPost]
        [Route("create/order")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderCreateModel orderCreateModel)
        {
            if (orderCreateModel == null) return BadRequest();
            var customerId = User.Identity.GetUserId<long>();

            var orderDTO = _mapper.Map<OrderDTO>(orderCreateModel);
            orderDTO.CustomerId = customerId;
            orderDTO.DeliveryType = orderCreateModel.DeliveryTypeEnum.ToString();
            orderDTO.PaymentType = orderCreateModel.PaymentTypeEnum.ToString();

            var orderId = await _orderService.CreateOrderAsync(orderDTO);
            if (orderId > ID_NOT_FOUND)
            {
                switch(orderCreateModel.PaymentTypeEnum) 
                {
                    case PaymentType.Card :
                    var sessionId = await _orderService.OrderProccessingCard(orderId);
                    if (sessionId == null)
                        return BadRequest();
                    return Ok(new { sessionId });

                    case PaymentType.Cash :
                    if(await _orderService.OrderProccessingCash(orderId))
                        return Ok(new { message = ProductsSuccesPayment });
                    return BadRequest();
                    default: return BadRequest();
                }
            }
            else return Conflict(new { errorText = ProductsBadPayment });
        }
    }
}
