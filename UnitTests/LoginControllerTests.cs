using Xunit;
using Microsoft.AspNetCore.Mvc;
using my_api_project.Controllers;
using my_api_project.Models;
using my_api_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace UnitTests
{
    public class LoginControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private IConfiguration GetConfig()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "Jwt:Key", "your_super_secret_key_here" }
            };
            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalidCredentials()
        {
            var context = GetDbContext();
            var controller = new LoginController(context, GetConfig());
            var request = new LoginRequest { UserName = "baduser", Password = "badpass" };
            var result = await controller.Login(request);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsToken_WhenValidCredentials()
        {
            var context = GetDbContext();
            context.CardHolders.Add(new CardHolder { CardHolderId = 1, UserName = "user", Password = "pass", First = "A", Last = "B" });
            context.SaveChanges();
            var controller = new LoginController(context, GetConfig());
            var request = new LoginRequest { UserName = "user", Password = "pass" };
            var result = await controller.Login(request);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("token", okResult.Value.ToString());
        }
    }
}
