using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Services
{
    public interface IServicesReadOnlyRepository
    {
        Task<List<Service>> GetAll(Entities.User user);  

        Task<Service?> GetById(Entities.User user, long id);

        Task<List<Service>> FilterByWeek(Entities.User user, DateOnly date);
    }
}
