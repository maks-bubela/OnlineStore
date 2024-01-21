using OnlineStore.DAL.Interfaces;

namespace OnlineStore.DAL.Entities
{
    public class BearerTokenSetting : IEntity
    {
        public long Id { get; set; }

        public byte LifeTime { get; set; }

        public long EnvironmentTypeId { get; set; }
        public virtual EnvironmentType EnvironmentType { get; private set; }
    }
}
