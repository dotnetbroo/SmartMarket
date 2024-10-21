using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.DTOs.Kassas;

namespace SmartMarket.Service.Interfaces.Kassas;

public interface IKassaService
{
    Task<bool> ReamoveAsync(long id);//
    Task<KassaForResultDto> RetrieveByIdAsync(long id);//
    Task<KassaForResultDto> CreateAsync(KassaForCreationDto dto);//
    Task<KassaForResultDto> ModifyAsync(KassaForUpdateDto dto);//
    Task<IEnumerable<KassaForResultDto>> RetrieveAllAsync(PaginationParams @params);//
}
