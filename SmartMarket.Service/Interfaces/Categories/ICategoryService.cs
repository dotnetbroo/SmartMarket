using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Categories;

namespace SmartMarket.Service.Interfaces.Categories;


public interface ICategoryService
{
    Task<bool> ReamoveAsync(long id);
    Task<CategoryForResultDto> RetrieveByIdAsync(long id);
    Task<CategoryForResultDto> CreateAsync(CategoryForCreationDto dto);
    Task<CategoryForResultDto> ModifyAsync(CategoryForUpdateDto dto);
    Task<IEnumerable<CategoryForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
