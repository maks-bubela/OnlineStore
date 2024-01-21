using OnlineStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.DTO
{
    public class OrderDTO
    {
        public long CustomerId { get; set; }
        public long ProductsId { get; set; }
        public long QuantityProducts { get; set; }
        public string PaymentType { get; set; }
        public string DeliveryType { get; set; }
    }
}
