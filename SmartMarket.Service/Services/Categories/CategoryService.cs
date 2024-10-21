using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.Interfaces.Categories;

namespace SmartMarket.Service.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryForResultDto> CreateAsync(CategoryForCreationDto dto)
    {
        var category = await _categoryRepository.SelectAll()
            .Where(c => c.Name.ToLower() == dto.Name.ToLower())
            .FirstOrDefaultAsync();
        if (category is not null)
            throw new CustomException(403, "Bu kategoriya bazada mavjud.");

        var mappedCategory = _mapper.Map<Category>(dto);
        mappedCategory.CreatedAt = DateTime.UtcNow;

        var result = await _categoryRepository.InsertAsync(mappedCategory);

        return _mapper.Map<CategoryForResultDto>(result);
    }

    public async Task<CategoryForResultDto> ModifyAsync(CategoryForUpdateDto dto)
    {
        var category = await _categoryRepository.SelectAll()
            .Where(c => c.Id == dto.Id)
            .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Kategoriya topilmadi.");

        var mappedCategory = _mapper.Map(dto, category);
        mappedCategory.UpdatedAt = DateTime.UtcNow;

        var result = await _categoryRepository.UpdateAsync(mappedCategory);

        return _mapper.Map<CategoryForResultDto>(result);
    }

    public async Task<bool> ReamoveAsync(long id)
    {
        var category = await _categoryRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Kategoriya topilmadi.");

        await _categoryRepository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<CategoryForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var categories = await _categoryRepository.SelectAll()
                .Include(c => c.Products)
                .Include(c => c.Cards)
                .Include(c => c.PartnersProducts)
                .AsNoTracking()
                .ToPagedList(@params)
                .ToListAsync();

        return _mapper.Map<IEnumerable<CategoryForResultDto>>(categories);
    }

    public async Task<CategoryForResultDto> RetrieveByIdAsync(long id)
    {
        var category = await _categoryRepository.SelectAll()
                .Where(c => c.Id == id)
                .Include(c => c.Products)
                .Include(c => c.PartnersProducts)
                .Include(c => c.Cards)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Kategoriya topilmadi.");

        return _mapper.Map<CategoryForResultDto>(category);
    }
}
