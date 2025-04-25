using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CashMate.Controllers;
using CashMate.Models;
using CashMate.Models.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CashMate.Tests
{

    public class AccountControllerTests
    {
        private Mock<ApplicationDbContext> _mockDbContext;
        private Mock<IConfiguration> _mockConfig;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockConfig = new Mock<IConfiguration>();

            // Mock HttpContext
            var httpContext = new Mock<HttpContext>();
            var session = new Mock<ISession>();
            httpContext.Setup(h => h.Session).Returns(session.Object);

            _controller = new AccountController(_mockDbContext.Object, _mockConfig.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext.Object
                }
            };
        }

        [Test]
        public void Register_ValidModel_ReturnsRedirectToConfirmEmail()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "test@test.com",
                Password = "Test1234",
                UserName = "TestUser"
            };

            _mockDbContext.Setup(db => db.Users).Returns((DbSet<User>)new List<User>().AsQueryable());

            // Act
            var result = _controller.Register(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ConfirmEmail", result.ActionName);
        }

        [Test]
        public void Register_DuplicateEmail_ReturnsRedirectToHomeWithError()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "test@test.com",
                Password = "Test1234",
                UserName = "TestUser"
            };

            var data = new List<User>
            {
              new User { Email = "test@test.com" }
            }.AsQueryable();
            var mockDbSet = CreateDbSetMock(data);
            _mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);

            // Act
            var result = _controller.Register(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);

            static Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> data) where T : class
            {
                var mockSet = new Mock<DbSet<T>>();
                mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
                mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
                mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
                mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
                return mockSet;
            }
        }
        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }
    }

}