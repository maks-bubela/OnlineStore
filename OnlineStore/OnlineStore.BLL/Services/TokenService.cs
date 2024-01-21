using OnlineStore.BLL.Enums;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace OnlineStore.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly OnlineStoreContext _ctx;
        public TokenService(OnlineStoreContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(OnlineStoreContext));
        }
        public async Task<int> GetTokenSettingsAsync(EnvirementTypes type)
        {
            var tokenSetting = await _ctx.Set<BearerTokenSetting>().Include(x => x.EnvironmentType)
                .Where(b => b.EnvironmentType.Id == ((long)type))
                .SingleOrDefaultAsync() ?? throw new EntityArgumentNullException(nameof(BearerTokenSetting));
            return tokenSetting.LifeTime;
        }
    }
}
