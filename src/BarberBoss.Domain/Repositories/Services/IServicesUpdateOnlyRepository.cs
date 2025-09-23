using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Services
{
    public interface IServicesUpdateOnlyRepository
    {
        Task<Service?> GetById(Domain.Entities.User user, long id);
        void Update(Service service);
    }
}
