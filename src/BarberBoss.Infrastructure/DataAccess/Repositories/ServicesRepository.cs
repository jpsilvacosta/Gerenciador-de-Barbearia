using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BarberBoss.Infrastructure.DataAccess.Repositories
{
    internal class ServicesRepository : IServicesWriteOnlyRepository, IServicesReadOnlyRepository, IServicesUpdateOnlyRepository
    {
        private readonly BarberBossDbContext _dbContext;
        public ServicesRepository(BarberBossDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task Add(Service service)
        {
            await _dbContext.Services.AddAsync(service);
        }
        public async Task<bool> Delete(long id)
        {
            var result = await _dbContext.Services.FirstOrDefaultAsync(service => service.Id == id);
            if (result is null)
            {
                return false;
            }
            _dbContext.Services.Remove(result);

            return true;
        }

        public async Task<List<Service>> FilterByWeek(User user, DateOnly date)
        {
            var currentDate = date.ToDateTime(TimeOnly.MinValue);

            int offset = ((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var startDate = currentDate.AddDays(-offset).Date;

            var endDate = startDate.AddDays(7).AddTicks(-1);

            return await _dbContext.Services.AsNoTracking()
                .Where(service => service.UserId == user.Id && service.Date >= startDate && service.Date <= endDate)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.ServiceType)
                .ToListAsync();
        }

        public async Task<List<Service>> GetAll(User user)
        {
            return await _dbContext.Services.AsNoTracking().Where(service => service.UserId == user.Id).ToListAsync();
        }

        async Task<Service?> IServicesReadOnlyRepository.GetById(User user, long id)
        {
            return await _dbContext.Services.AsNoTracking().FirstOrDefaultAsync(service => service.Id == id);
        }

        async Task<Service?> IServicesUpdateOnlyRepository.GetById(User user, long id)
        {
            return await _dbContext.Services.FirstOrDefaultAsync(service => service.Id == id && service.UserId == user.Id);
        }

        public void Update(Service service)
        {
            _dbContext.Services.Update(service);
        }

        
    }
}
