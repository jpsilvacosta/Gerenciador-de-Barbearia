namespace BarberBoss.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        Task<Entities.User> GetById(long id);

        Task Update(Entities.User user);
    }
}
