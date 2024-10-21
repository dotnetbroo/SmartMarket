using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.Commons.Helpers;
using SmartMarket.Service.DTOs.Products;
using SmartMarket.Service.Interfaces.Categories;
using SmartMarket.Service.Interfaces.Products;
using SmartMarket.Service.Interfaces.Users;

namespace SmartMarket.Service.Services.Products;

public class ProductStoryService : IProductStoryService
{
    private readonly IMapper _mapper;
    private readonly IRepository<ProductStory> _productStoryRepository;

    public ProductStoryService(IMapper mapper, IRepository<ProductStory> productStoryRepository)
    {
        _mapper = mapper;
        _productStoryRepository = productStoryRepository;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var product = await _productStoryRepository.SelectAll()
            .Where(p => p.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (product is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        var fullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, product.ImagePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return await _productStoryRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProductStoryForResultDto>> GetAllAsync(PaginationParams @params)
    {
        var products = await _productStoryRepository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ProductStoryForResultDto>>(products);
    }

    public async Task<ProductStoryForResultDto> GetByIdAsync(long id)
    {
        var product = await _productStoryRepository.SelectAll()
             .Where(p => p.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (product is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        return _mapper.Map<ProductStoryForResultDto>(product);
    }
}
