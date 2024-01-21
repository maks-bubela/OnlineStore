using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<long> RegisterCustomerAsync(CustomerDTO customerDTO);
        Task<bool> VerifyCredentialsAsync(string username, string password);
    }
}
