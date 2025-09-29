using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Entities;
using Bogus;

namespace CommonTestUtilities.Entities
{
    public class ServiceBuilder
    {
        public static List<Service> Collection(User user, uint count = 2)
        {
            var list = new List<Service>();

            if (count == 0)
                count = 1;

            var serviceId = 1;

            for (int i = 0; i < count; i++)
            {
                var service = Build(user);
                service.Id = serviceId++;

                list.Add(service);
            }

            return list;
        }

        public static Service Build(User user, DateTime? fixedDate = null)
        {
            return new Faker<Service>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.ServiceType, f => f.PickRandom<ServiceType>())
            .RuleFor(u => u.Description, f => f.Finance.ToString())
            .RuleFor(u => u.Date, faker => faker.Date.Past())
            .RuleFor(u => u.Amount, f => f.Random.Decimal(min: 1, max: 1000))
            .RuleFor(u => u.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(u => u.UserId, _ => user.Id)
            .Generate();
        }

    }
}
