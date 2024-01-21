using Microsoft.Extensions.Configuration;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Configuration;
using Stripe;
using Stripe.Checkout;

namespace OnlineStore.Services;
public class StripeService : IStripeService
{
    private readonly IConfiguration _configuration;

    private const string UsdCurrency = "usd";
    private const string CheckoutMode = "payment";
    private const string PaymentMethodTypesCard = "card";
    private const string SuccessPaymentUrl = "https://your-website.com/success";
    private const string CancelPaymentUrl = "https://your-website.com/cancel";



    public StripeService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> CreateCheckoutSessionAsync(ProductsDTO productsDTO)
    {
        StripeConfiguration.ApiKey = OnlineStoreConfiguration.CreateFromConfigurations().StripeSecretKey;

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { PaymentMethodTypesCard },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = UsdCurrency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = productsDTO.ProductName,
                        },
                        UnitAmount = UnitAmountCalculate(productsDTO.Price),
                    },
                    Quantity = productsDTO.Quantity,
                },
            },
            Mode = CheckoutMode,
            SuccessUrl = SuccessPaymentUrl,
            CancelUrl = CancelPaymentUrl,
        };

        var service = new SessionService();
        var session = service.Create(options);

        return session.Id;
    }

    // This method calculate usd to cents
    private long UnitAmountCalculate(long price)
    {
        return price * 100;
    }
}