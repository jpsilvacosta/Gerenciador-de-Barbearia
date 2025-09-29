using BarberBoss.Application.UseCases.Services.Reports.Pdf;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ServiceIdentityManager Service_Admin { get; private set; } = default!;
        public ServiceIdentityManager Service_MemberTeam { get; private set; } = default!;
        public UserIdentityManager User_Team_Member { get; private set; } = default!;
        public UserIdentityManager User_Admin { get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<BarberBossDbContext>(config =>
                    {
                        config.UseInMemoryDatabase(Guid.NewGuid().ToString());
                        config.UseInternalServiceProvider(provider);
                    });

                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IGenerateServicesReportPdfUseCase));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddScoped<IGenerateServicesReportPdfUseCase, FakeGenerateServicesReportPdfUseCase>();

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BarberBossDbContext>();
                    var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
                    var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    StartDatabase(dbContext, passwordEncrypter, accessTokenGenerator);
                });
        }

        private void StartDatabase(
            BarberBossDbContext dbContext,
            IPasswordEncrypter passwordEncrypter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var userTeamMember = AddUserTeamMember(dbContext, passwordEncrypter, accessTokenGenerator);
            var serviceTeamMember = AddServices(dbContext, userTeamMember, serviceId: 1);
            Service_MemberTeam = new ServiceIdentityManager(serviceTeamMember);

            var userAdmin = AddUserAdmin(dbContext, passwordEncrypter, accessTokenGenerator);
            var serviceAdmin = AddServices(dbContext, userAdmin, serviceId: 2);
            Service_Admin = new ServiceIdentityManager(serviceAdmin);

            dbContext.SaveChanges();
        }

        private User AddUserTeamMember(
            BarberBossDbContext dbContext,
            IPasswordEncrypter passwordEncrypter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            foreach (var entry in dbContext.ChangeTracker.Entries<User>().ToList())
            {
                entry.State = EntityState.Detached;
            }

            var user = UserBuilder.Build();
            user.Id = 1;

            var password = user.Password;
            user.Password = passwordEncrypter.Encrypt(user.Password);

            var existing = dbContext.Users.SingleOrDefault(u => u.Id == 1);
            if (existing != null)
            {
                dbContext.Users.Remove(existing);
                dbContext.SaveChanges();
            }

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var token = accessTokenGenerator.Generate(user);

            User_Team_Member = new UserIdentityManager(user, password, token);

            return user;
        }

        private User AddUserAdmin(
            BarberBossDbContext dbContext,
            IPasswordEncrypter passwordEncrypter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            var user = UserBuilder.Build(Roles.ADMIN);
            user.Id = 2;

            var password = user.Password;
            user.Password = passwordEncrypter.Encrypt(password);

            dbContext.Users.Add(user);

            var token = accessTokenGenerator.Generate(user);
            User_Admin = new UserIdentityManager(user, password, token);

            return user;
        }

        private Service AddServices(
           BarberBossDbContext dbContext,
           User user,
           long serviceId)
        {
            var service = ServiceBuilder.Build(user);
            service.Id = serviceId;

            dbContext.Services.Add(service);
            return service;
        }
    }
}
