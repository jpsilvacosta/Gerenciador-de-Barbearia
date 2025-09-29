using BarberBoss.Application.UseCases.Services.Reports.Pdf;

namespace WebApi.Test.Resources
{
    public class FakeGenerateServicesReportPdfUseCase : IGenerateServicesReportPdfUseCase
    {
        public Task<byte[]> Execute(DateOnly week)
        {
            byte[] fakePdf = new byte[] { 37, 80, 68, 70 }; //PDF em ASCII
            return Task.FromResult(fakePdf);    
        }
    }
}
