using Bogus;
using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestServiceJsonBuilder
    {
        public static RequestServiceJson Build()
        {
            return new Faker<RequestServiceJson>()
                .RuleFor(r => r.ServiceType, faker => faker.PickRandom<ServiceType>())
                .RuleFor(r => r.Description, faker => faker.Finance.TransactionType())
                .RuleFor(r => r.Date, faker => faker.Date.Past())
                .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
                .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>());
        }
    }
}
