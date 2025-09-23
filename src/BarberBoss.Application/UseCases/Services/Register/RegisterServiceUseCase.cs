using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Services.Register
{
    public class RegisterServiceUseCase : IRegisterServiceUseCase
    {
        private readonly IServicesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterServiceUseCase(IServicesWriteOnlyRepository repository, IUnitOfWork unitOfWork,IMapper mapper, ILoggedUser loggedUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }
        public async Task<ResponseRegisteredServiceJson> Execute(RequestServiceJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.Get();

            var service = _mapper.Map<Service>(request);
            service.UserId = loggedUser.Id;
            
            await _repository.Add(service);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredServiceJson>(service);

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
