using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses
{
    public class ResponseServiceJson
    {
        public long Id { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }

    }
}
