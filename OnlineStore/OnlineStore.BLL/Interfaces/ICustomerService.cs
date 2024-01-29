using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> SoftDeleteAsync(long id);
        Task<bool> CustomerExistsAsync(string username);
        Task<bool> CustomerExistsAsync(long id);
        Task<CustomerDTO> GetCustomerByIdAsync(long id);
        Task<CustomerDTO> GetCustomerByUsernameAsync(string username);
        Task<List<CustomerDTO>> GetCustomersListAsync();
    }
}
