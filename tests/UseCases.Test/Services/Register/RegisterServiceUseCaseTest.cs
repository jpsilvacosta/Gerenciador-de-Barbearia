using BarberBoss.Application.UseCases.Services.Register;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Services.Register
{
    public class RegisterServiceUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var request = RequestServiceJsonBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();
        }

        private RegisterServiceUseCase CreateUseCase(User user)
        {
            var repository = ServicesWriteOnlyRepositoryBuilder.Build();
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new RegisterServiceUseCase(repository, unitOfWork, mapper, loggedUser);
        }
    }
}
