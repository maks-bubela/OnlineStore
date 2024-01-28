using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Entities;
using System.Linq.Expressions;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository _repository;

        private const long ID_NOT_FOUND = 0;

        public CustomerService(IMapper mapper, IGenericRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        #region Predicates
        private Expression<Func<Customer, bool>> GetActiveCustomerByIdPredicate(long customerId)
        {
            Expression<Func<Customer, bool>> predicate = x => x.Id == customerId && !x.IsDelete;
            return predicate;
        }
        private Expression<Func<Customer, bool>> GetActiveCustomerByUsernamePredicate(string username)
        {
            Expression<Func<Customer, bool>> predicate = x => x.Username == username && !x.IsDelete;
            return predicate;
        }
        private Expression<Func<Customer, bool>> GetActiveCustomerPredicate()
        {
            Expression<Func<Customer, bool>> predicate = x => !x.IsDelete;
            return predicate;
        }

        #endregion

        #region Read Methods
        public async Task<CustomerDTO> GetCustomerByIdAsync(long id)
        {
            if (id <= ID_NOT_FOUND) throw new EntityArgumentNullException(nameof(id));

            var predicate = GetActiveCustomerByIdPredicate(id);
            var customer = await _repository.GetAsync(predicate, x => x.Role);
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return customerDTO;
        }

        public async Task<CustomerDTO> GetCustomerByUsernameAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));

            var predicate = GetActiveCustomerByUsernamePredicate(username);
            var customer = await _repository.GetAsync(predicate, x => x.Role);
            if (customer == null) throw new NullReferenceException(nameof(customer));

            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return customerDTO;
        }

        public async Task<List<CustomerDTO>> GetCustomersListAsync()
        {
            var predicate = GetActiveCustomerPredicate();
            var customersList = await _repository.ListAsync(predicate, x => x.Role);

            if (customersList == null) throw new NullReferenceException(nameof(customersList));

            var userDTOList = _mapper.Map<List<CustomerDTO>>(customersList);
            return userDTOList;
        }

        public async Task<bool> CustomerExistsAsync(long id)
        {
            var predicate = GetActiveCustomerByIdPredicate(id);
            var exists = await _repository.IsExistAsync(predicate);
            return exists;
        }
        #endregion

        #region Delete Methods
        public async Task<bool> SoftDeleteAsync(long id)
        {
            if (!await CustomerExistsAsync(id)) throw new NotFoundInDatabaseException();

            var predicate = GetActiveCustomerByIdPredicate(id);
            var customer = await _repository.GetAsync(predicate);
            if (customer != null)
            {
                customer.IsDelete = true;
                await _repository.UpdateAsync(customer);
                return true;
            }
            return false;
        }
        #endregion
    }
}
