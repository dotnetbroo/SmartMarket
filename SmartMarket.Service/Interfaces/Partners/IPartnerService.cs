using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Partners;

namespace SmartMarket.Service.Interfaces.Partners;

public interface IPartnerService
{
    Task<bool> RemoveAsync(long id);//
    Task<PartnerForResultDto> RetrieveByIdAsync(long id);//
    Task<PartnerForResultDto> CreateAsync(PartnerForCreationDto dto);//
    Task<PartnerForResultDto> ModifyAsync(PartnerForUpdateDto dto);//
    Task<IEnumerable<PartnerForResultDto>> RetrieveAllAsync(PaginationParams @params);//
}