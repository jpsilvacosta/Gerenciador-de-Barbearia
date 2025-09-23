using BarberBoss.Domain.Repositories.Services;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ServicesWriteOnlyRepositoryBuilder
    {
        public static IServicesWriteOnlyRepository Build()
        {
            var mock = new Mock<IServicesWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
