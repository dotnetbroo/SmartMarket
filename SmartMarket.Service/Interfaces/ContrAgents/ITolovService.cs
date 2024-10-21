using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.ContrAgents;
using SmartMarket.Service.DTOs.Tolov;

namespace SmartMarket.Service.Interfaces.ContrAgents;

public interface ITolovService
{
    Task<bool> RemoveAsync(long id);//
    Task<TolovForResultDto> RetrieveByIdAsync(long id);//
    Task<IEnumerable<TolovForResultDto>> RetrieveAllAsync(PaginationParams @params);//
}
