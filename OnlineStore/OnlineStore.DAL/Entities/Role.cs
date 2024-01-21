using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Entities
{
    public class Role : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; private set; } = new HashSet<Customer>();
    }
}
