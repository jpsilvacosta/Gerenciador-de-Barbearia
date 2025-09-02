using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Requests
{
    public class RequestServiceJson
    {
        public ServiceType ServiceType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }

    }
}
