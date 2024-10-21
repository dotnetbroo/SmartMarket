using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Tolov;

namespace SmartMarket.Service.Interfaces.Partners;

public interface IPartnerTolovService
{
    Task<bool> RemoveAsync(long id);//
    Task<TolovForResultDto> RetrieveByIdAsync(long id);//
    Task<IEnumerable<TolovForResultDto>> RetrieveAllAsync(PaginationParams @params);//
}
