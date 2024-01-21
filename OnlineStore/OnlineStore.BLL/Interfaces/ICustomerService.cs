using OnlineStore.BLL.DTO;
using OnlineStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> SoftDeleteAsync(long id);
        Task<bool> CustomerExistsAsync(long id);
        Task<CustomerDTO> GetCustomerByIdAsync(long id);
        Task<CustomerDTO> GetCustomerByUsernameAsync(string username);
        Task<List<CustomerDTO>> GetCustomersListAsync();

        Expression<Func<Customer, bool>> CreateActiveCustomerPredicate(long customerId);
    }
}
