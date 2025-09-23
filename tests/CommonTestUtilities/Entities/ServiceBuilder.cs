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
            .RuleFor(u => u.Id, _ => 0)
            .RuleFor(u => u.ServiceType, f => f.PickRandom<ServiceType>())
            .RuleFor(u => u.Date, _ => fixedDate ?? DateTime.UtcNow.Date)
            .RuleFor(u => u.Amount, f => f.Random.Decimal(min: 1, max: 1000))
            .RuleFor(u => u.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(u => u.UserId, _ => user.Id)
            .Generate();
        }

        public static List<Service> BuildForMonth(User user, DateTime fixedDate)
        {
            var services = new List<Service>();

            for (int week = 0; week < 4; week++)
            {
                var date = new DateTime(fixedDate.Year, fixedDate.Month, 1)
                    .AddDays(week * 7);

                services.Add(Build(user, date));
            }

            return services;
        }
    }
}
