using Microsoft.AspNetCore.Mvc.Filters;
using BarberBoss.Exception.ExceptionsBase;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using BarberBoss.Exception;

namespace BarberBoss.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BarberBossException)
            {
                HandleProjectExpcetion(context);
            } 
            else
            {
                ThrowUnknownError(context);
            }
        }

        private void HandleProjectExpcetion(ExceptionContext context)
        {
            var barberBossException = (BarberBossException)context.Exception;
            var errorResponse = new ResponseErrorJson(barberBossException.GetErrors());

            context.HttpContext.Response.StatusCode = barberBossException.StatusCode;
            context.Result = new ObjectResult(errorResponse);
        }

        private void ThrowUnknownError(ExceptionContext context)
        {
            var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR + $"\n\nDetails: {context.Exception.Message}");

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; 

            context.Result = new ObjectResult(errorResponse);
        }
    }
}
