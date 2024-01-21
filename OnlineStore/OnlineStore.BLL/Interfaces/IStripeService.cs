using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(ProductsDTO productsDTO);
    }
}
