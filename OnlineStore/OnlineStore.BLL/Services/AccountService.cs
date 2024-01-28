using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Enums;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL.Interfaces;
using System.Linq.Expressions;


namespace OnlineStore.BLL.Services
{
    public class AccountService : IAccountService
    {
        #region Services
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly IGenericRepository _repository;
        private readonly IPasswordProcessing _passProcess;
        #endregion

        #region Predicates
        private Expression<Func<Role, bool>> GetRoleByNamePredicate(string customerRoleName)
        {
            Expression<Func<Role, bool>> predicate = a => a.Name == customerRoleName;
            return predicate;
        }
        private Expression<Func<Customer, bool>> GetActiveCustomerPredicate(string customerUsername)
        {
            Expression<Func<Customer, bool>> predicate = a => a.Username == customerUsername && !a.IsDelete;
            return predicate;
        }
        #endregion

        public AccountService(IMapper mapper, IPasswordProcessing passProcess, 
            ICustomerService customerService, IGenericRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passProcess = passProcess ?? throw new ArgumentNullException(nameof(passProcess));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        #region Create methods
        public async Task<long> RegisterCustomerAsync(CustomerDTO customerDTO)
        {
            if (customerDTO == null) throw new ArgumentException(nameof(customerDTO));
            if (await _customerService.CustomerExistsAsync(customerDTO.Id)) throw new DataExistsInDatabaseException();
            var salt = _passProcess.GenerateSalt();

            var predicate = GetRoleByNamePredicate(customerDTO.RoleName);
            var role = await _repository.GetAsync(predicate);
            if (role == null)
            {
                role = await _repository.
                    GetAsync<Role>((long)Roles.Customer) ?? throw new NullReferenceException(nameof(role));
            }

            var customer = _mapper.Map<Customer>(customerDTO);
            customer.Password = _passProcess.GetHashCode(customerDTO.Password, salt);
            customer.Salt = salt;
            customer.RoleId = role.Id;
            customer.IsDelete = false;
            await _repository.AddAsync(customer);

            if (!await _customerService.CustomerExistsAsync(customer.Id)) throw new FailedAddToDatabaseException();
            return customer.Id;
        }
        #endregion

        #region Read methods
        public async Task<bool> VerifyCredentialsAsync(string username, string password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var predicate = GetActiveCustomerPredicate(username);
            var customer = await _repository.GetAsync(predicate);
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
