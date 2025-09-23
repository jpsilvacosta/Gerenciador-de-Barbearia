﻿using BarberBoss.Application.UseCases.Services.Delete;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;


namespace UseCases.Test.Services.Delete
{
    public class DeleteServiceUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var service = ServiceBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, service);

            var act = async () => await useCase.Execute(service.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Service_Not_Found()
        {
            var loggedUser = UserBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: 1000);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.SERVICE_NOT_FOUND));
        }

        private DeleteServiceUseCase CreateUseCase(User user, Service? service = null)
        {
            var respoitoryWriteOnly = ServicesWriteOnlyRepositoryBuilder.Build();
            var respository = new ServicesReadOnlyRepositoryBuilder().GetById(user, service).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteServiceUseCase(respoitoryWriteOnly, unitOfWork, loggedUser, respository);
        }
    }
}
