using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Services.GetAll
{
    public class GetAllServicesUseCase : IGetAllServicesUseCase
    {
        private readonly IServicesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetAllServicesUseCase(IServicesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseServicesJson> Execute()
        {
            var loggedUser = await _loggedUser.Get();

            var result = await _repository.GetAll(loggedUser);

            return new ResponseServicesJson
            {
                Services = _mapper.Map<List<ResponseShortServiceJson>>(result)
            };


        }
    }
}
