using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using BarberBoss.Domain.Services.LoggedUser;
using System.Runtime.CompilerServices;

namespace BarberBoss.Application.UseCases.Services.Delete
{
    public class DeleteServiceUseCase : IDeleteServiceUseCase
    {
        private readonly IServicesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;
        private readonly IServicesReadOnlyRepository _servicesReadOnly;

        public DeleteServiceUseCase(
            IServicesWriteOnlyRepository repository, 
            IUnitOfWork unitOfWork,
            ILoggedUser loggedUser,
            IServicesReadOnlyRepository servicesReadOnly)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
            _servicesReadOnly = servicesReadOnly;
        }

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.Get();

            var services = await _servicesReadOnly.GetById(loggedUser, id);

            if (services is null)
            {
                throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);
            }

            await _repository.Delete(id);

            await _unitOfWork.Commit();

        }
    }
}
