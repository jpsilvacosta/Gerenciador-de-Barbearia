using BarberBoss.Application.UseCases.Services.Reports.Pdf;
using BarberBoss.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBoss.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class ReportController : ControllerBase
    {
        [HttpGet("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> GetPdf(
            [FromServices] IGenerateServicesReportPdfUseCase useCase,
            [FromQuery] DateOnly week)
        {
            byte[] file = await useCase.Execute(week);

            if (file.Length > 0)
                return File(file, MediaTypeNames.Application.Pdf, "report-pdf");

            return NoContent();
        }
    }
}
