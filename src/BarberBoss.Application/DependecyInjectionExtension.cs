using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Login.DoLogin;
using BarberBoss.Application.UseCases.Services.Delete;
using BarberBoss.Application.UseCases.Services.GetAll;
using BarberBoss.Application.UseCases.Services.GetServiceById;
using BarberBoss.Application.UseCases.Services.Register;
using BarberBoss.Application.UseCases.Services.Reports.Pdf;
using BarberBoss.Application.UseCases.Services.Update;
using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Application.UseCases.Users.Delete;
using BarberBoss.Application.UseCases.Users.Profile;
using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Application.UseCases.Users.Update;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application
{
    public static class DependecyInjectionExtension
    {

        public static void AddApplication(this IServiceCollection services)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterServiceUseCase, RegisterServiceUseCase>();
            services.AddScoped<IDeleteServiceUseCase, DeleteServiceUseCase>();
            services.AddScoped<IGetAllServicesUseCase, GetAllServicesUseCase>();
            services.AddScoped<IGetServiceByIdUseCase, GetServiceByIdUseCase>();
            services.AddScoped<IUpdateServiceUseCase, UpdateServiceUseCase>(); 
            services.AddScoped<IGenerateServicesReportPdfUseCase, GenerateServicesReportPdfUseCase>(); 
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>(); 
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>(); 
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        }
    }
}
