using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Domains.Core;
using CareSphere.Services.Organizations.Interfaces;
using CareSphere.Services.Users.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace CareSphere.Services.Tests.Users
{
    [TestFixture]
    public class UserServiceTests : TestBase
    {
        private IUserService _userService;
        [SetUp]
        public void Initialize()
        {
            _userService = ServiceProvider.GetRequiredService<IUserService>();
        }
        [Test]
        public async Task CreateUser_ShouldAddUser_WhenUserDoesNotExist()
        {
            // Arrange
            var newUser = new User
            {
                Username = "newuser",
                Email = "newuser@example.com",
                CreatedAt = DateTime.UtcNow,
                Name = "New User"

            };

            // Act
            var result = await _userService.CreateUser(newUser);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(newUser.Email));
        }
    }
}
