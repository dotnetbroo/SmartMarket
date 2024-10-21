using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Enums;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.Partners;

namespace SmartMarket.Service.Interfaces.PartnerProducts;

public interface IPartnerProductService
{
    Task<bool> RemoveAsync(long id);//
    Task<PartnerProductForResultDto> RetrieveByIdAsync(long id);//
    Task<PartnerProductForResultDto> RetrieveByTransNoAsync(string transNo);//
    Task<IEnumerable<PartnerProductForResultDto>> RetrieveAllAsync(PaginationParams @params);//
    Task<PartnerForResultDto> PayForProductsAsync(long partnerId, decimal paid, long tolovUsuli);//
    Task<IEnumerable<PartnerProductForResultDto>> RetrieveAllWithDateTimeAsync(long userId, DateTime startDate, DateTime endDate);//
}
