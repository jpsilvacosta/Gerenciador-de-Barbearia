using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Services;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ServicesReadOnlyRepositoryBuilder
    {
        private readonly Mock<IServicesReadOnlyRepository> _repository;

        public ServicesReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IServicesReadOnlyRepository>();
        }

        public ServicesReadOnlyRepositoryBuilder GetAll(User user, List<Service> services)
        {
            _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(services);

            return this;
        }

        public ServicesReadOnlyRepositoryBuilder GetById(User user, Service? service)
        {
            if (service is not null)
                _repository.Setup(repository => repository.GetById(user, service.Id)).ReturnsAsync(service);

            return this;
        }

        public ServicesReadOnlyRepositoryBuilder FilterByWeek(User user, List<Service> services)
        {
            _repository.Setup(repository => repository.FilterByWeek(user, It.IsAny<DateOnly>())).ReturnsAsync(services);

            return this;
        }

        public IServicesReadOnlyRepository Build() => _repository.Object;
    }
}
