using OnlineStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Entities
{
    public class Products : IEntity
    {
        public long Id { get; set; }
        public string ProductName { set; get; }
        public string Description { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public virtual ICollection<Order> Orders { get; private set; } = new HashSet<Order>();
    }
}
