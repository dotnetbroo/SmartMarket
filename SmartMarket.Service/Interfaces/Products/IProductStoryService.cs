using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Service.DTOs.Products;

namespace SmartMarket.Service.Interfaces.Products;

public interface IProductStoryService
{
    Task<bool> DeleteAsync(long id);//
    Task<ProductStoryForResultDto> GetByIdAsync(long id);//
    Task<IEnumerable<ProductStoryForResultDto>> GetAllAsync(PaginationParams @params);//
}
