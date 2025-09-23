using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberBoss.Application.UseCases.Services.Delete
{
    public interface IDeleteServiceUseCase
    {
        Task Execute(long id);
    }
}
