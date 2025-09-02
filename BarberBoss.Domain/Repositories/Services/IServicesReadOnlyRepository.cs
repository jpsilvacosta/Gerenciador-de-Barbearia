using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Services
{
    public  interface IServicesReadOnlyRepository
    {
        Task<List<Service>> GetAll();  

        Task<Service?> GetById(long id);

        Task<List<Service>> FilterByWeek(DateOnly date);
    }
}
