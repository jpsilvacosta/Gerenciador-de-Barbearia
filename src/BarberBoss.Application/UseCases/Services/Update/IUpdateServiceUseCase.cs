using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberBoss.Application.UseCases.Services.Update
{
    public interface IUpdateServiceUseCase
    {
        Task Execute(long id, RequestServiceJson request);
    }
}
