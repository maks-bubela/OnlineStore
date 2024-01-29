using AutoMapper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OnlineStore.BLL.Cryptography;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.MappingProfiles;
using OnlineStore.BLL.Services;
using OnlineStore.DAL.Context;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Repository;

namespace OnlineStore.Tests.UnitTests.BLLTests
{
    [TestFixture]
    public class AccountServicesTest
    {
        private IMapper _mapper;
        private CustomerDTO _customerDTO;
        private ICustomerService _customerService;
        private IGenericRepository _repository;
        private IAccountService _accService;
        private IPasswordProcessing _passProcessing;
        private OnlineStoreContext _ctx;

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

        #region Registration Tests
        [Test, Order(0)]
        public async Task RegisterCustomerAsync_ReturnsTrue()
        {
            var customerId = await _accService.RegisterCustomerAsync(_customerDTO);
            Assert.That(customerId, Is.Not.EqualTo(0));
        }
        #endregion

        #region Veryfying Credentials Tests
        [Test, Order(2)]
        public async Task VerifyCredentialsAsync_VeryfyingExistingCredentials_ReturnsTrue()
        {
            await _accService.RegisterCustomerAsync(_customerDTO);
            var loginResult = await _accService.VerifyCredentialsAsync(_customerDTO.Username, _customerDTO.Password);
            Assert.That(loginResult, Is.True);
        }

        [Test, Order(3)]
        public async Task VerifyCredentialsAsync_VeryfyingNotExistingCredentials_ReturnsFalse()
        {
            await _accService.RegisterCustomerAsync(_customerDTO);
            _customerDTO.Password = "ANOTHERPASS";
            var loginResult = await _accService.VerifyCredentialsAsync(_customerDTO.Username, _customerDTO.Password);
            Assert.That(loginResult, Is.False);
        }
        #endregion
    }
}