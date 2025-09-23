using BarberBoss.Application.UseCases.Services.GetAll;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Services.GetAll
{
    public class GetAllServiceUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var service = ServiceBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, service);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Services.Should().NotBeNullOrEmpty().And.AllSatisfy(service =>
            {
                service.Id.Should().BeGreaterThan(0);
                service.ServiceType.Should().Be(expected: service.ServiceType);
                service.Amount.Should().BeGreaterThan(0);
            });
        }

        private GetAllServicesUseCase CreateUseCase(User user, List<Service> services)
        {
            var repository = new ServicesReadOnlyRepositoryBuilder().GetAll(user, services).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllServicesUseCase(repository, mapper, loggedUser);
        }
    }
}
