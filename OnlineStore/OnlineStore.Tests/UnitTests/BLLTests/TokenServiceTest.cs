using Microsoft.EntityFrameworkCore;
using OnlineStore.BLL.Enums;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.Services;
using OnlineStore.DAL.Context;
using NUnit.Framework;
using OnlineStore.DAL.Repository;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.Tests.UnitTests.BLLTests
{
    [TestFixture]
    class TokenServiceTest
    {
        #region Services
        private ITokenService _tokenService;
        private IGenericRepository _repository;
        private OnlineStoreContext _ctx;
        #endregion

        #region Constants
        const EnvirementTypes _envirementTypes = EnvirementTypes.Testing;
        #endregion

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OnlineStoreContext>();
            optionsBuilder.UseInMemoryDatabase(nameof(OnlineStoreContext));
            _ctx = new OnlineStoreContext(optionsBuilder.Options);
            _repository = new GenericRepository(_ctx);
            _tokenService = new TokenService(_repository);
        }
        #region GetMethodTest
        [Test]
        public async Task GetTokenSettingsAsync__FoundData_NotNullResult()
        {
            var data = await _tokenService.GetTokenSettingsAsync(_envirementTypes);
            Assert.That(data, Is.Not.Null);
        }
        #endregion
    }
}
