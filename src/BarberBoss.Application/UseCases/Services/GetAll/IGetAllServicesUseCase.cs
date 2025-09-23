using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Services.GetAll
{
    public interface IGetAllServicesUseCase
    {
        Task<ResponseServicesJson> Execute();
    }
}
