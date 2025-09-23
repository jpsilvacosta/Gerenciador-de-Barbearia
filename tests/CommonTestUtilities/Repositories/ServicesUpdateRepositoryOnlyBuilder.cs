using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Services;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ServicesUpdateRepositoryOnlyBuilder
    {
        private readonly Mock<IServicesUpdateOnlyRepository> _repository;

        public ServicesUpdateRepositoryOnlyBuilder()
        {
            _repository = new Mock<IServicesUpdateOnlyRepository>();
        }

        public ServicesUpdateRepositoryOnlyBuilder GetById(User user, Service? service)
        {
            if (service is not null)
                _repository.Setup(repository => repository.GetById(user, service.Id)).ReturnsAsync(service);

            return this;
        }

        public IServicesUpdateOnlyRepository Build() => _repository.Object;
    }
}
