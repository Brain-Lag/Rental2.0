using FluentAssertions;
using Rental.Models;
using Rental.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental2._0.Tests
{
    public class AccountServiceTests : IClassFixture<DbContextFixture>
    {
        private readonly AccountService _accountService;
        private readonly DbContextFixture _fixture;

        public AccountServiceTests(DbContextFixture fixture)
        {
            _fixture = fixture;
            _accountService = new AccountService(_fixture.Context);
        }

        [Theory]
        [InlineData("MySuperSecretPassword123")]
        public void Password_Encrypt_Success(string password)
        {
            var encryptedPassword = _accountService.EncryptPassword(password);

            encryptedPassword.Should().NotBe(password);
        }

        [Fact]
        public async Task Register_Success()
        {
            var user = new User
            {
                UserName = "test123",
                UserEmail = "test123@example.com",
                UserPassword = "TestPassword123",
            };

            var result = await _accountService.Register(user);

            result.Should().BeTrue();
            var registeredUser = _fixture.Context.User.FirstOrDefault(u => u.UserName == user.UserName);
            registeredUser.Should().NotBeNull();
            registeredUser!.UserEmail.Should().Be(user.UserEmail);
        }

        [Fact]
        public void Login_Success()
        {
            var user = new User
            {
                UserName = "testuser",
                UserEmail = "testuser@example.com",
                UserPassword = "TestPassword123"
            };

            user.UserPassword = _accountService.EncryptPassword(user.UserPassword);
            _fixture.Context.User.Add(user);
            _fixture.Context.SaveChanges();

            var loginResult = _accountService.Login(new User
            {
                UserName = "testuser",
                UserPassword = "TestPassword123"
            });

            loginResult.Item1.Should().BeTrue();
        }

        [Fact]
        public void Login_Failure()
        {
            var user = new User
            {
                UserName = "testuser",
                UserEmail = "testuser@example.com",
                UserPassword = "TestPassword123"
            };

            user.UserPassword = _accountService.EncryptPassword(user.UserPassword);
            _fixture.Context.User.Add(user);
            _fixture.Context.SaveChanges();

            var loginResult = _accountService.Login(new User
            {
                UserName = "testuser",
                UserPassword = "WrongPassword"
            });

            loginResult.Item1.Should().BeFalse();
        }
    }

}
