using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Services.GetServiceById
{
    public class GetServiceByIdUseCase : IGetServiceByIdUseCase
    {
        private readonly IServicesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetServiceByIdUseCase(IServicesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }
        public async Task<ResponseServiceJson> Execute(long id)
        {
           var loggedUser = await _loggedUser.Get();

            var result = await _repository.GetById(loggedUser, id);
              if (result is null)
                throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

              return _mapper.Map<ResponseServiceJson>(result);
        }
    }
}
