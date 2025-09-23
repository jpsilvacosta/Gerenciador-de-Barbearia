using BarberBoss.Application.UseCases.Services.GetServiceById;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Services.GetById
{
    public class GetServiceByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var service = ServiceBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, service);

            var result = await useCase.Execute(service.Id);
            
            result.Should().NotBeNull();
            result.Id.Should().Be(service.Id);
            result.ServiceType.Should().Be((BarberBoss.Communication.Enums.ServiceType)service.ServiceType);
            result.Date.Should().Be(service.Date);
            result.Amount.Should().Be(service.Amount);
            result.PaymentType.Should().Be((BarberBoss.Communication.Enums.PaymentType)service.PaymentType);
        }

        [Fact]
        public async Task Error_Service_Not_Found()
        {
            var loggedUser = UserBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: 1000);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(BarberBoss.Exception.ResourceErrorMessages.SERVICE_NOT_FOUND));
        }

        private GetServiceByIdUseCase CreateUseCase(User user, Service? service = null)
        {
            var repository = new ServicesReadOnlyRepositoryBuilder().GetById(user, service).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetServiceByIdUseCase(repository, mapper, loggedUser);
        }
    }
}
