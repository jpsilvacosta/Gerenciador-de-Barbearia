using BarberBoss.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberBoss.Application.UseCases.Services.GetServiceById
{
    public interface IGetServiceByIdUseCase
    {
        Task<ResponseServiceJson> Execute(long id);
    }
}
