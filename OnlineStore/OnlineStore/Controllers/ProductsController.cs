using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        #region Services
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly ICustomerService _customerService;
        #endregion

        public ProductsController(IProductsService productsService, ICustomerService customerService, IMapper mapper)
        {
            _productsService = productsService ?? throw new ArgumentNullException(nameof(IProductsService));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(ICustomerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
        }

        /// <summary>
        /// Gets authenticated products info (name, price, quantity and description,).
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Info was found </response>>
        /// <response code="500"> Something is wrong on server </response>>
        [Authorize, HttpPost]
        [Route("products/info")]
        public async Task<IActionResult> GetProductsByIdAsync(long productsId)
        {
            var productsDTO = await _productsService.GetAvailableProductsByIdAsync(productsId);
            if (productsDTO == null) return StatusCode(500);

            var productsInfo = _mapper.Map<ProductsInfoModel>(productsDTO);
            return Ok(new { productsInfo });
        }

        /// <summary>
        /// Gets authenticated products list info (name, price, quantity and description,).
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Info was found </response>>
        /// <response code="500"> Something is wrong on server </response>>
        [Authorize, HttpPost]
        [Route("products/list/info")]
        public async Task<IActionResult> GetListProductsAsync()
        {
            var productsListDTO = await _productsService.GetAvailableProductsListAsync();
            if (productsListDTO == null) return StatusCode(500);

            var productsListInfo = _mapper.Map<List<ProductsInfoModel>>(productsListDTO);
            return Ok(new { productsListInfo });
        }
    }
}
