using BarberBoss.Application.UseCases.Services.Update;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using BarberBoss.Exception;

namespace UseCases.Test.Services.Update
{
    public class UpdateServiceUseCaseTest
    {

        [Fact]

        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var request = RequestServiceJsonBuilder.Build();
            var service = ServiceBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, service);

            var act = async () => await useCase.Execute(service.Id, request);

            await act.Should().NotThrowAsync();

            service.ServiceType.Should().Be((BarberBoss.Domain.Enums.ServiceType?)request.ServiceType);
            service.Description.Should().Be(request.Description);
            service.Date.Should().Be(request.Date);
            service.Amount.Should().Be(request.Amount);
            service.PaymentType.Should().Be((BarberBoss.Domain.Enums.PaymentType)request.PaymentType);
        }

        [Fact]

        public async Task Error_ServiceType_Empty()
        {
            var loggedUser = UserBuilder.Build();
            var service = ServiceBuilder.Build(loggedUser);

            var request = RequestServiceJsonBuilder.Build();
            request.ServiceType = null;
            
            var useCase = CreateUseCase(loggedUser, service);

            var act = async () => await useCase.Execute(service.Id, request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.SERVICE_TYPE_REQUIRED));
        }

        [Fact]

        public async Task Error_Service_Not_Found()
        {
            var loggedUser = UserBuilder.Build();

            var request = RequestServiceJsonBuilder.Build();

            var useCase = CreateUseCase(loggedUser, null);

            var act = async () => await useCase.Execute(id: 1000, request);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.SERVICE_NOT_FOUND));
        }

        private UpdateServiceUseCase CreateUseCase(User user, Service? Service = null)
        {
            var repository = new ServicesUpdateRepositoryOnlyBuilder().GetById(user, Service).Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);


            return new UpdateServiceUseCase(repository, unitOfWork, mapper, loggedUser);
        }
    }
}
