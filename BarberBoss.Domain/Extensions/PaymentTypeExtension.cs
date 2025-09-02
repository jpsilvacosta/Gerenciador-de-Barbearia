using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports;

namespace BarberBoss.Domain.Extensions
{
    public static class PaymentTypeExtension
    {

        public static string PaymentTypeToString(this PaymentType paymentType)
        {
            return paymentType switch
            {
                PaymentType.Cash => ResourceReportGenerationMessages.CASH,
                PaymentType.CreditCard => ResourceReportGenerationMessages.CREDIT_CARD,
                PaymentType.DebitCard => ResourceReportGenerationMessages.DEBIT_CARD,
                PaymentType.ApplePay => ResourceReportGenerationMessages.APPLE_PAY,
                PaymentType.PIX => ResourceReportGenerationMessages.PIX,
                _ => string.Empty,
            };
        }
    }
}
