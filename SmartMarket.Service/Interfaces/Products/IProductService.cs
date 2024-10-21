using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Service.DTOs.Products;

namespace SmartMarket.Service.Interfaces.Products;

public interface IProductService
{
    string GeneratePCode();
    Task<bool> DeleteAsync(long id);//
    Task<ProductForResultDto> GetByIdAsync(long id);//
    void UpdatePriceAndPercentage(Product product, ProductForCreationDto dto);
    Task<IEnumerable<ProductForResultDto>> GetAllAsync(PaginationParams @params);//
    Task<ProductForResultDto> CreateAsync(ProductForCreationDto productForCreationDto);//
    Task<ProductForResultDto> UpdateAsync(ProductForUpdateDto productForUpdateDto);//
}
