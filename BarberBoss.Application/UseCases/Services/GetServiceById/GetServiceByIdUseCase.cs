using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Services.GetServiceById
{
    public class GetServiceByIdUseCase : IGetServiceByIdUseCase
    {
        private readonly IServicesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetServiceByIdUseCase(IServicesReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResponseServiceJson> Execute(long id)
        {
           var result = await _repository.GetById(id);
              if (result is null)
                throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

              return _mapper.Map<ResponseServiceJson>(result);
        }
    }
}
