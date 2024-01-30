using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Enums;
using OnlineStore.BLL.Interfaces;
using OnlineStore.ExtensionMethods;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using System.Security.Claims;

namespace OnlineStore.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        #region Constants
        private const string InvalidUserData = "Invalid username or password.";
        private const string InvalidModel = "Invalid input model.";
        private const string CacheTokenAccess = "accessToken";
        #endregion

        #region Services
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IAuthOptions _authOptions;
        private readonly ITokenService _tokenService;
        private readonly EnvirementTypes _envirementType;
        private readonly ICustomerService _userService;
        private readonly IDistributedCache _cache;
        #endregion

        public AuthenticationController(ITokenService tokenService, IAccountService accountService,
            IMapper mapper, IAuthOptions authOptions, ICustomerService userService, IDistributedCache cache)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(IAccountService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
            _authOptions = authOptions ?? throw new ArgumentNullException(nameof(IAuthOptions));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(ITokenService));
            _envirementType = EnvirementTypes.Development;
            _userService = userService ?? throw new ArgumentNullException(nameof(ICustomerService));
            _cache = cache ?? throw new ArgumentNullException(nameof(IDistributedCache));

    }

        /// <summary>
        /// Authenticates a customer and returns generated token if authentication is successful
        /// </summary>
        /// <param name="customer">Customer and login information to authenticate</param>
        /// <returns></returns>
        /// <response code="200"> Customer is authenticated </response>
        /// <response code="409"> Authentication is failed </response>
        /// <response code="204"> Not founded environment type </response>
        /// <response code="400">Model not valid</response>    
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> TokenAsync([FromBody] CustomerLoginModel customer)
        {
            if (ModelState.IsValid)
            {
                var verified = await _accountService.VerifyCredentialsAsync(customer.Username, customer.Password);
                if (!verified)
                    return Conflict(new { errorText = InvalidUserData });
                var identity = await GetIdentityAsync(customer);
                var lifeTime = await _tokenService.GetTokenSettingsAsync(_envirementType);
                if (lifeTime == 0)
                    return NoContent();
                var encodedJwt = await GetTokenAsync(identity, lifeTime, customer.Username);
                var response = new
                {
                    access_token = encodedJwt
                };
                return Json(response);
            }
            return BadRequest(new { errorText = InvalidUserData });
        }

        /// <summary>
        /// Registration new customer and add him into database
        /// </summary>
        /// <param name="customerRegist">Customer and login information to authenticate</param>
        /// <param name="roleName">Customer role name</param>
        /// <returns></returns>
        /// <response code="200"> Customer is authenticated </response>
        /// <response code="409"> Authentication is failed </response>
        /// <response code="400"> Model not valid </response>    
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> RegistrationAsync(
            [FromBody] CustomerRegistrationModel customerRegist, [FromQuery] string roleName)
        {
            if (ModelState.IsValid && customerRegist != null)
            {
                var customerDTO = _mapper.Map<CustomerDTO>(customerRegist);
                customerDTO.RoleName = roleName;

                var customerId = await _accountService.RegisterCustomerAsync(customerDTO);
                if (customerId != 0)
                {
                    var response = new
                    {
                        Username = customerDTO.Username
                    };
                    return Json(response);
                }
                return Conflict();
            }
            return BadRequest(new { errorText = InvalidModel });
        }

        // Private method that return claims identity about user
        private async Task<ClaimsIdentity> GetIdentityAsync(CustomerLoginModel customerModel)
        {
            var verified = await _accountService.VerifyCredentialsAsync(customerModel.Username, customerModel.Password);
            if (verified)
            {
                var customerDTO = await _userService.GetCustomerByUsernameAsync(customerModel.Username);
                if (customerDTO != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, customerDTO.Username),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, customerDTO.RoleName),
                        new Claim(ClaimTypes.NameIdentifier, customerDTO.Id.ToString())
                    };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
            }
            return null;
        }

        private async Task<string> GetTokenAsync(ClaimsIdentity identity, int lifeTime, string username)
        {
            var tokenSettingsDto = new TokenSettingsDTO()
            {
                Identity = identity,
                LifeTime = lifeTime
            };
            var cachedToken = await _cache.GetStringAsync(username + CacheTokenAccess);
            if (cachedToken == null)
            {
                var encodedJwt = _authOptions.GetSymmetricSecurityKey(tokenSettingsDto);
                await _cache.SetStringWithExpirationAsync(username + CacheTokenAccess, encodedJwt, TimeSpan.FromMinutes(lifeTime));
                return encodedJwt;
            }
            return cachedToken;
        }
    }
}
