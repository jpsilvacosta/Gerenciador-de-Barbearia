using BarberBoss.Communication.Requests;
using FluentValidation;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Services
{
    public class ServiceValidator : AbstractValidator<RequestServiceJson>
    {
        public ServiceValidator()
        {
            RuleFor(expense => expense.ServiceType).IsInEnum().WithMessage(ResourceErrorMessages.SERVICE_TYPE_INVALID);
            RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.SERVICES_CANNOT_FOR_THE_FUTURE);
            RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
            RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);

        }
    }
}
