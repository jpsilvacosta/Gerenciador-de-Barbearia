using AutoMapper;
using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using Microsoft.Extensions.Options;

namespace BarberBoss.Application.UseCases.Services.Update
{
    public class UpdateServiceUseCase : IUpdateServiceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServicesUpdateOnlyRepository _repository;
        private readonly IMapper _mapper;

        public UpdateServiceUseCase(IServicesUpdateOnlyRepository repository, IUnitOfWork unitOfWork,IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;   
            _mapper = mapper;
        }
        public async Task Execute(long id, RequestServiceJson request)
        {
            Validate(request);

            var service = await _repository.GetById(id);

            if (service is null)
                throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);

            _mapper.Map(request, service);

            _repository.Update(service);

            await _unitOfWork.Commit();
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
