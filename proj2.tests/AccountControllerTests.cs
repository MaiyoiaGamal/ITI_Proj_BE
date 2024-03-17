using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using proj2.Controllers;
using proj2.DTO;
using proj2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj2.tests
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Registration_ValidModel_ReturnsOk()
        {
            // Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var identityOptionsMock = new Mock<IOptions<IdentityOptions>>();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                identityOptionsMock.Object,
                null, null, null, null, null, null, null);

            var configMock = new Mock<IConfiguration>();
            var controller = new AccountController(userManagerMock.Object, configMock.Object);
            var registerDto = new RegisterDTO
            {
                Email = "test@example.com",
                UserName = "testuser",
                Password = "TestPassword123@"
            };

            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Registertion(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Account successfully added", okResult.Value);
            userManagerMock.VerifyAll();

        }
    }
}
