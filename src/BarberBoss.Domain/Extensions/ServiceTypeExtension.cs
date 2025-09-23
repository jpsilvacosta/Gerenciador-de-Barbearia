using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports;

namespace BarberBoss.Domain.Extensions
{
    public static class ServiceTypeExtension
    {
        public static string ServiceTypeToString(this ServiceType serviceType)
        {
            return serviceType switch
            {
                ServiceType.Haircut => ResourceReportGenerationMessages.HAIRCUT,
                ServiceType.Shave => ResourceReportGenerationMessages.SHAVE,
                ServiceType.BeardAndHaircut => ResourceReportGenerationMessages.BEARD_AND_HAIRCUT,
                ServiceType.BeardTrim => ResourceReportGenerationMessages.BEARD_TRIM,
                _ => string.Empty,
            };
        }
    }
}
