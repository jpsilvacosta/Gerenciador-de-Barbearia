using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses
{
    public class ResponseShortServiceJson
    {
        public long Id { get; set; }
        public ServiceType ServiceType { get; set; }
        public decimal Amount { get; set; }
    }
}
