using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Services;

namespace BarberBoss.Application.UseCases.Services.GetAll
{
    public class GetAllServicesUseCase : IGetAllServicesUseCase
    {
        private readonly IServicesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllServicesUseCase(IServicesReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseServicesJson> Execute()
        {
            var result = await _repository.GetAll();

            return new ResponseServicesJson
            {
                Services = _mapper.Map<List<ResponseShortServiceJson>>(result)
            };


        }
    }
}
