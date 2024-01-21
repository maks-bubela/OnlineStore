using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Entities
{
    public class Customer : IEntity
    {
        public long Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public long RoleId { get; set; }
        public virtual Role Role { get; private set; }
        public virtual ICollection<Order> Orders { get; private set; } = new HashSet<Order>();
        public bool IsDelete { get; set; }
        public string Salt { get; set; }

    }
}
