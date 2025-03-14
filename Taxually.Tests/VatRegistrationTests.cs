using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Interface;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;

namespace Taxually.Tests
{
    public class VatRegistrationTests
    {
        private readonly Mock<IVatHandler> _vatServiceMock;
        private readonly VatRegistrationController _controller;

        public VatRegistrationTests()
        {
            _vatServiceMock = new Mock<IVatHandler>();
            _controller = new VatRegistrationController(_vatServiceMock.Object);
        }

        [Fact]
        public async Task Post_ValidRequest_ReturnsOk()
        {
            var request = new VatRegistrationRequest { CompanyName = "TestCo", CompanyId = "12345", Country = "GB" };
            _vatServiceMock.Setup(v => v.RegisterVatAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.Post(request, CancellationToken.None);

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Post_InvalidCountry_ReturnsNotFound()
        {
            var request = new VatRegistrationRequest { CompanyName = "TestCo", CompanyId = "12345", Country = "XX" };
            _vatServiceMock.Setup(v => v.RegisterVatAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var result = await _controller.Post(request, CancellationToken.None);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task RegisterVatAsync_ValidRequest_ReturnsTrue()
        {
            var validatorMock = new Mock<IValidator<VatRegistrationRequest>>();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<VatRegistrationRequest>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult());
            var loggerMock = new Mock<ILogger<VatService>>();
            var strategyMock = new Mock<IVatRegistrationStrategy>();
            strategyMock.Setup(s => s.RegisterAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            strategyMock.Setup(s => s.CountryCode).Returns("GB");

            var strategies = new List<IVatRegistrationStrategy> { strategyMock.Object };
            var service = new VatService(strategies, validatorMock.Object, loggerMock.Object);

            var result = await service.RegisterVatAsync(new VatRegistrationRequest { Country = strategyMock.Object.CountryCode }, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task RegisterVatAsync_InvalidRequest_ReturnsFalse()
        {
            var validatorMock = new Mock<IValidator<VatRegistrationRequest>>();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<VatRegistrationRequest>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Country", "Invalid country") }));
            var loggerMock = new Mock<ILogger<VatService>>();
            var strategies = new List<IVatRegistrationStrategy>();
            var service = new VatService(strategies, validatorMock.Object, loggerMock.Object);

            var result = await service.RegisterVatAsync(new VatRegistrationRequest { Country = "XX" }, CancellationToken.None);

            Assert.False(result);
        }
    }
}