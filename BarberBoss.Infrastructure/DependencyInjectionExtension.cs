﻿using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServicesWriteOnlyRepository, ServicesRepository>();
            services.AddScoped<IServicesReadOnlyRepository, ServicesRepository>();
            services.AddScoped<IServicesUpdateOnlyRepository, ServicesRepository>();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BarberBossDatabase");

            var version = new Version(8, 0, 43);
            var serverVersion = new MySqlServerVersion(version);

            services.AddDbContext<BarberBossDbContext>(config => config.UseMySql(connectionString, serverVersion));
        }
    }
}
