using Bogus;
using NUnit.Framework;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.Cryptography;

namespace OnlineStore.Tests.UnitTests.BLLTests
{
    [TestFixture]
    public class PasswordProcessingTest
    {
        private IPasswordProcessing _passProcessing;
        private string _testPassword;
        private string _salt;

        [SetUp]
        public void SetUp()
        {
            _passProcessing = new PasswordProcessing();
            _testPassword = new Faker().Internet.Password(length: 6);
            _salt = _passProcessing.GenerateSalt();
        }

        [Test, Order(0)]
        public void GetHashCode_GettingHashCode_ReturnsTrue()
        {
            var generatedHashCode =  _passProcessing.GetHashCode(_testPassword, _salt);
            var testGeneratedHashCode = _passProcessing.GetHashCode(_testPassword, _salt);

            Assert.That(generatedHashCode, Is.EqualTo(testGeneratedHashCode));
        }

        [Test, Order(1)]
        public void GetHashCode_GettingHashCodeWithDifferentSalt_ReturnsFalse()
        {
            var generatedHashCode = _passProcessing.GetHashCode(_testPassword, _salt);

            var testSalt = _passProcessing.GenerateSalt();
            var testGeneratedHashCode = _passProcessing.GetHashCode(_testPassword, testSalt);

            Assert.That(generatedHashCode, Is.Not.EqualTo(testGeneratedHashCode));
        }

        [Test, Order(2)]
        public void GetHashCode_ComparingHashCodesWithDifferentPasswords_ReturnsTrue()
        {
            var secondPassword = new Faker().Internet.Password(length: 6);
            var generatedHashCode = _passProcessing.GetHashCode(_testPassword, _salt);
            var testGeneratedHashCode = _passProcessing.GetHashCode(secondPassword, _salt);

            Assert.That(generatedHashCode, Is.Not.EqualTo(testGeneratedHashCode));
        }

        [Test, Order(3)]
        public void GetHashCode_NullArguments_ThrowsNullReferenceException()
        {
            Assert.Throws<ArgumentException>(
                () => _passProcessing.GetHashCode(null, null));
        }


        [Test, Order(4)]
        public void GetHashCode_EmptyArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => _passProcessing.GetHashCode("", ""));
        }

        [Test, Order(5)]
        public void GetHashCode_WhiteSpaceArguments_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => _passProcessing.GetHashCode(" ", " "));
        }
    }
}
