using Xunit;
using Microsoft.AspNetCore.Mvc;
using my_api_project.Controllers;
using my_api_project.Models;
using my_api_project.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class TransactionControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetTransactions_ReturnsAllTransactions()
        {
            var context = GetDbContext();
            context.Transactions.Add(new Transaction { TransactionId = 1 });
            context.Transactions.Add(new Transaction { TransactionId = 2 });
            context.SaveChanges();
            var controller = new TransactionController(context);
            var result = await controller.GetTransactions();
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task GetTransaction_ReturnsTransaction_WhenExists()
        {
            var context = GetDbContext();
            context.Transactions.Add(new Transaction { TransactionId = 1 });
            context.SaveChanges();
            var controller = new TransactionController(context);
            var result = await controller.GetTransaction(1);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.TransactionId);
        }

        [Fact]
        public async Task GetTransaction_ReturnsNotFound_WhenNotExists()
        {
            var context = GetDbContext();
            var controller = new TransactionController(context);
            var result = await controller.GetTransaction(99);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateTransaction_AddsTransaction()
        {
            var context = GetDbContext();
            var controller = new TransactionController(context);
            var transaction = new Transaction { TransactionId = 1 };
            var result = await controller.CreateTransaction(transaction);
            Assert.Equal(1, context.Transactions.Count());
        }
    }
}
