using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string DeliveryType { get; set; }
        public string PaymentType { get; set; }
        public long QuantityProducts { get; set; }
        public string? SessionId { get; set; }
        public virtual Customer Customer { get; private set; }
        public long ProductsId { get; set; }
        public virtual Products Products { get; private set; }
    }
}
