using BarberBoss.Communication.Requests;
using FluentValidation;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Services
{
    public class ServiceValidator : AbstractValidator<RequestServiceJson>
    {
        public ServiceValidator()
        {
            RuleFor(service => service.ServiceType).IsInEnum().WithMessage(ResourceErrorMessages.SERVICE_TYPE_INVALID).NotNull().WithMessage(ResourceErrorMessages.SERVICE_TYPE_REQUIRED);
            RuleFor(service => service.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.SERVICES_CANNOT_FOR_THE_FUTURE);
            RuleFor(service => service.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
            RuleFor(service => service.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);

        }
    }
}
