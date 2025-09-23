using BarberBoss.Domain.Entities;

namespace WebApi.Test.Resources
{
    public class ServiceIdentityManager
    {
        private readonly Service _service;
        public ServiceIdentityManager(Service service)
        {
            _service = service;
        }

        public long GetId() => _service.Id;

        public DateTime GetDate() => _service.Date;
    }
}
