namespace BarberBoss.Application.UseCases.Services.Reports.Pdf
{
    public interface IGenerateServicesReportPdfUseCase
    {
        Task<byte[]> Execute(DateOnly week);    
    }
}
