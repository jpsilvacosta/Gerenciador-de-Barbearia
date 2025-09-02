using BarberBoss.Application.UseCases.Services;
using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Services.Register
{
    public class RegisterServiceValidatorTests
    {
        [Fact]

        public void Success()
        {
            //Arrange
            var validator = new ServiceValidator();
            var request = RequestRegisterServiceJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Service_Type_Invalid()
        {
            //Arrange
            var validator = new ServiceValidator();
            var request = RequestRegisterServiceJsonBuilder.Build();
            request.ServiceType = (ServiceType)200;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.SERVICE_TYPE_INVALID));
        }

        [Fact]

        public void Error_Date_Future()
        {
            //Arrange
            var validator = new ServiceValidator();
            var request = RequestRegisterServiceJsonBuilder.Build();
            request.Date = DateTime.UtcNow.AddDays(2);

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage
            .Equals(ResourceErrorMessages.SERVICES_CANNOT_FOR_THE_FUTURE));
        }

        [Fact]
        public void Error_Payment_Type_Invalid()
        {
            // Arrange
            var validator = new ServiceValidator();
            var request = RequestRegisterServiceJsonBuilder.Build();
            request.PaymentType = (PaymentType)300;


            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage
            .Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Error_Amount_Invalid(decimal amount)
        {
            // Arrange
            var validator = new ServiceValidator();
            var request = RequestRegisterServiceJsonBuilder.Build();
            request.Amount = amount;


            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage
            .Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
        }
    }
}
