using AutoMapper;
using System.Security.Cryptography;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<RequestServiceJson, Service>();
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, config => config.Ignore());
        }

        private void EntityToResponse()
        {
            CreateMap<Service, ResponseRegisteredServiceJson>();
            CreateMap<Service, ResponseShortServiceJson>();
            CreateMap<Service, ResponseServiceJson>();
            CreateMap<User, ResponseUserProfileJson>();
        }
    }
}
