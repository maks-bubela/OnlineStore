using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OnlineStore.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly OnlineStoreContext _ctx;

        private const long ID_NOT_FOUND = 0;

        public CustomerService(IMapper mapper, OnlineStoreContext ctx)
        {
            _mapper = mapper ?? throw new ArgumentNullException();
            _ctx = ctx ?? throw new ArgumentNullException();
        }

        #region Predicates
        public Expression<Func<Customer, bool>> CreateActiveCustomerPredicate(long customerId)
        {
            Expression<Func<Customer, bool>> predicate = x => x.Id == customerId && !x.IsDelete;
            return predicate;
        }

        #endregion

        #region Read Methods
        public async Task<CustomerDTO> GetCustomerByIdAsync(long id)
        {
            if (id <= ID_NOT_FOUND) throw new EntityArgumentNullException(nameof(id));

            var predicate = CreateActiveCustomerPredicate(id);
            var customer = await _ctx.Set<Customer>().Where(predicate)
                .Include(x => x.Role)
                .SingleOrDefaultAsync();
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return customerDTO;
        }

        public async Task<CustomerDTO> GetCustomerByUsernameAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));

            var customer = await _ctx.Set<Customer>().Include(x => x.Role)
                .Where(x => x.Username == username)
                .SingleOrDefaultAsync();
            if (customer == null) throw new NullReferenceException(nameof(customer));

            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return customerDTO;
        }

        public async Task<List<CustomerDTO>> GetCustomersListAsync()
        {
            var customersList = await _ctx.Set<Customer>().Include(x => x.Role)
                .Where(x => !x.IsDelete).ToListAsync();
            if (customersList == null) throw new NullReferenceException(nameof(customersList));

            var userDTOList = _mapper.Map<List<CustomerDTO>>(customersList);
            return userDTOList;
        }

        public async Task<bool> CustomerExistsAsync(long id)
        {
            var predicate = CreateActiveCustomerPredicate(id);
            var exists = await _ctx.Set<Customer>().AnyAsync(predicate);
            return exists;
        }
        #endregion

        #region Delete Methods
        public async Task<bool> SoftDeleteAsync(long id)
        {
            if (!await CustomerExistsAsync(id)) throw new NotFoundInDatabaseException();

            var predicate = CreateActiveCustomerPredicate(id);
            var customer = await _ctx.Set<Customer>().Where(predicate)
                .SingleOrDefaultAsync();
            if (customer != null)
            {
                customer.IsDelete = true;
                _ctx.Entry(customer).State = EntityState.Modified;
                await _ctx.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
    }
}
