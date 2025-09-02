using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Services.Register
{
    public class RegisterServiceUseCase : IRegisterServiceUseCase
    {
        private readonly IServicesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterServiceUseCase(IServicesWriteOnlyRepository repository, IUnitOfWork unitOfWork,IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseRegisteredServiceJson> Execute(RequestServiceJson request)
        {
            Validate(request);

            var entity = _mapper.Map<Service>(request);
            
            await _repository.Add(entity);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredServiceJson>(entity);

        }

        private void Validate(RequestServiceJson request)
        {
            var validator = new ServiceValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
