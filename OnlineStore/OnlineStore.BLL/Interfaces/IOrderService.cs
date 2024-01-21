using OnlineStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<long> CreateOrderAsync(OrderDTO orderDTO);
        Task<string> OrderProccessingCard(long orderId);
        Task<bool> OrderProccessingCash(long orderId);
    }
}
