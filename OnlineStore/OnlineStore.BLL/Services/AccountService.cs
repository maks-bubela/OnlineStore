using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Enums;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace OnlineStore.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly OnlineStoreContext _ctx;
        private readonly IPasswordProcessing _passProcess;

        public AccountService(OnlineStoreContext ctx, IMapper mapper,
            IPasswordProcessing passProcess, ICustomerService customerService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _passProcess = passProcess ?? throw new ArgumentNullException(nameof(passProcess));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        #region Create methods
        public async Task<long> RegisterCustomerAsync(CustomerDTO customerDTO)
        {
            if (customerDTO == null) throw new ArgumentException(nameof(customerDTO));
            if (await _customerService.CustomerExistsAsync(customerDTO.Id)) throw new DataExistsInDatabaseException();

            var salt = _passProcess.GenerateSalt();
            var role = await _ctx.Set<Role>().Where(x => x.Name == customerDTO.RoleName)
                .SingleOrDefaultAsync();
            if (role == null)
            {
                role = await _ctx.Set<Role>().Where(x => x.Id == (long)Roles.Customer)
                    .SingleOrDefaultAsync() ?? throw new NullReferenceException(nameof(role));
            }

            var customer = _mapper.Map<Customer>(customerDTO);
            customer.Password = _passProcess.GetHashCode(customerDTO.Password, salt);
            customer.Salt = salt;
            customer.RoleId = role.Id;
            customer.IsDelete = false;

            _ctx.Set<Customer>().Add(customer);
            await _ctx.SaveChangesAsync();
            if (!await _customerService.CustomerExistsAsync(customer.Id)) throw new FailedAddToDatabaseException();
            return customer.Id;
        }
        #endregion

        #region Read methods
        public async Task<bool> VerifyCredentialsAsync(string username, string password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var customer = await _ctx.Set<Customer>().Where(x => x.Username == username && !x.IsDelete)
                    .SingleOrDefaultAsync();
            if (customer != null)
            {
                string pass = _passProcess.GetHashCode(password, customer.Salt);
                if (customer.Password == pass)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
