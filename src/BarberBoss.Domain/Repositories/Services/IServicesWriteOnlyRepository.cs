using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Services
{
    public interface IServicesWriteOnlyRepository
    {
        Task Add(Service service);

        Task<bool> Delete(long id);
    }
}
