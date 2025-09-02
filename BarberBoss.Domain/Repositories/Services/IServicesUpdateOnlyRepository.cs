using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Services
{
    public interface IServicesUpdateOnlyRepository
    {
        Task<Service?> GetById(long id);
        void Update(Service service);
    }
}
