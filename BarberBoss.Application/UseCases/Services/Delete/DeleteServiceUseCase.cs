using BarberBoss.Domain.Repositories.Services;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Services.Delete
{
    public class DeleteServiceUseCase : IDeleteServiceUseCase
    {
        private readonly IServicesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceUseCase(IServicesWriteOnlyRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id)
        {
            var result = await _repository.Delete(id);

            if (!result)
            {
                throw new NotFoundException(ResourceErrorMessages.SERVICE_NOT_FOUND);
            }
            await _unitOfWork.Commit();

        }
    }
}
