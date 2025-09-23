using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using BarberBoss.Application.UseCases.Services.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Application.UseCases.Services.Delete;
using BarberBoss.Application.UseCases.Services.GetAll;
using BarberBoss.Application.UseCases.Services.GetServiceById;
using BarberBoss.Application.UseCases.Services.Update;
using Microsoft.AspNetCore.Authorization;

namespace BarberBoss.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredServiceJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Register([FromServices] IRegisterServiceUseCase useCase,
            [FromBody] RequestServiceJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Delete([FromServices] IDeleteServiceUseCase useCase,
            [FromRoute] long id)
        {
            await useCase.Execute(id);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseServicesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> GetAllServices([FromServices] IGetAllServicesUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Services.Count != 0)
            {
                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseServiceJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById([FromServices] IGetServiceByIdUseCase useCase,
            [FromRoute] long id)
        {
            var response = await useCase.Execute(id);

            if(response is not null)
            {
                return Ok(response);
            }

            return NotFound();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson),StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Update([FromServices] IUpdateServiceUseCase useCase,
            [FromRoute] long id,
            [FromBody] RequestServiceJson request)
        {
            await useCase.Execute(id, request);
            return NoContent();
        }
    }
}
