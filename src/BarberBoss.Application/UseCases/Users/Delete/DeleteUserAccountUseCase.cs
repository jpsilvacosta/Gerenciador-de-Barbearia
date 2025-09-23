using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.User;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Users.Delete
{
    public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserAccountUseCase(
            ILoggedUser loggedUser,
            IUserWriteOnlyRepository repository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute()
        {
            var user = await _loggedUser.Get();

            await _repository.Delete(user);

            await _unitOfWork.Commit();
        }
    }
}
