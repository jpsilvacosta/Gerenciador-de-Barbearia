using BarberBoss.Application.UseCases.Services.Reports.Pdf;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Services.Reports
{
    public class GenerateServicesReportPdfUseCaseTest
    {
        [Fact]

        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var services = ServiceBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, services);

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Success_Empty()
        {
            var loggedUser = UserBuilder.Build();

            var useCase = CreateUseCase(loggedUser, []);

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

            result.Should().BeEmpty();
        }



        private GenerateServicesReportPdfUseCase CreateUseCase(User user, List<Service> services)
        {
            var repository = new ServicesReadOnlyRepositoryBuilder().FilterByWeek(user, services).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GenerateServicesReportPdfUseCase(repository, loggedUser);
        }
    }
}
