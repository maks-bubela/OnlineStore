using Bogus;
using AutoMapper;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using OnlineStore.BLL.DTO;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using OnlineStore.BLL.Services;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.Exceptions;
using OnlineStore.BLL.Cryptography;
using OnlineStore.BLL.MappingProfiles;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Repository;

namespace OnlineStore.Tests.UnitTests.BLLTests
{
    [TestFixture]
    public class CustomerServiceTest
    {
        private IMapper _mapper;
        private CustomerDTO _customerDTO;
        private ICustomerService _customerService;
        private IAccountService _accService;
        private IGenericRepository _repository;
        private OnlineStoreContext _ctx;
        private IPasswordProcessing _passProcessing;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OnlineStoreContext>();
            optionsBuilder.UseInMemoryDatabase(nameof(OnlineStoreContext));
            _ctx = new OnlineStoreContext(optionsBuilder.Options);
            _passProcessing = new PasswordProcessing();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new CustomerProfileBLL()));
            _mapper = new Mapper(configuration);

            _repository = new GenericRepository(_ctx);
            _customerService = new CustomerService(_mapper, _repository);
            _accService = new AccountService(_mapper, _passProcessing, _customerService, _repository);
            _ctx.Database.EnsureCreated();

            var roles = new[] { "customer", "admin", "staff" };
            _customerDTO = new Faker<CustomerDTO>()
                .RuleFor(x => x.Username, r => r.Internet.UserName())
                .RuleFor(x => x.Password, r => r.Internet.Password(6, false, "", ""))
                .RuleFor(x => x.Firstname, r => r.Person.FirstName)
                .RuleFor(x => x.Lastname, r => r.Person.LastName)
                .RuleFor(x => x.Email, r => r.Person.Email)
                .RuleFor(x => x.RoleName, r => r.PickRandom(roles)).Generate();
        }

        #region Soft Delete Tests
        [Test, Order(0)]
        public async Task SoftDeleteAsync_RemovingExistingCustomer_ReturnsTrue()
        {
            var customerId = await _accService.RegisterCustomerAsync(_customerDTO);

            var isDeleted = await _customerService.SoftDeleteAsync(customerId);
            Assert.That(isDeleted, Is.True);
        }

        [Test, Order(1)]
        public async Task SoftDeleteAsync_RemovingNotExistingCustomer_ReturnsNotFoundInDatabaseException()
        {
            Assert.ThrowsAsync<NotFoundInDatabaseException>(async () =>
                await _customerService.SoftDeleteAsync(0));
        }

        [Test, Order(2)]
        public async Task SoftDeleteAsync_RemovingAlreadyDeletedCustomer_ReturnsNotFoundInDatabaseException()
        {
            var customerId = await _accService.RegisterCustomerAsync(_customerDTO);
            await _customerService.SoftDeleteAsync(customerId);

            Assert.ThrowsAsync<NotFoundInDatabaseException>(async () =>
                await _customerService.SoftDeleteAsync(customerId));
        }
        #endregion

        #region GetUserById Tests
        [Test, Order(3)]
        public async Task GetCustomerByIdAsync_GettingCustomerInfoById_ReturnsCustomer()
        {
            var customerId = await _accService.RegisterCustomerAsync(_customerDTO);

            var userInfoDTO = await _customerService.GetCustomerByIdAsync(customerId);
            Assert.That(userInfoDTO, Is.Not.Null);
        }

        [Test, Order(4)]
        public async Task GetCustomerByIdAsync_GettingNotExistingCustomerInfoById_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<EntityArgumentNullException>(async () =>
                await _customerService.GetCustomerByIdAsync(0));
        }
        #endregion

        #region GetCustomerByUsernameAsync Tests
        [Test, Order(5)]
        public async Task GetCustomerByUsernameAsync_GettingCustomerInfo_ReturnsCustomer()
        {
            await _accService.RegisterCustomerAsync(_customerDTO);
            var userInfo = await _customerService.GetCustomerByUsernameAsync(_customerDTO.Username);
            Assert.That(userInfo, Is.Not.Null);
        }

        [Test, Order(6)]
        public async Task GetCustomerByUsernameAsync_GettingNotExistingCustomerInfo_ThrowsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _customerService.GetCustomerByUsernameAsync(username: "notexisting"));
        }

        [Test, Order(7)]
        public async Task GetCustomerByUsernameAsync_PuttingNullArgument_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _customerService.GetCustomerByUsernameAsync(null));
        }
        #endregion

        #region CustomerExistsAsync Tests
        [Test, Order(8)]
        public async Task CustomerExistsAsync_PuttingExistingId_ReturnsTrue()
        {
            var customerId = await _accService.RegisterCustomerAsync(_customerDTO);
            var exists = await _customerService.CustomerExistsAsync(customerId);
            Assert.That(exists, Is.True);
        }

        [Test, Order(8)]
        public async Task CustomerExistsAsync_PuttingNotExistingId_ReturnsFalse()
        {
            var exists = await _customerService.CustomerExistsAsync(0);
            Assert.That(exists, Is.False);
        }
        #endregion
    }
}
