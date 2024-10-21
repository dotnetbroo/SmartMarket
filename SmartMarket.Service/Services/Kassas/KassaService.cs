using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.DTOs.Kassas;
using SmartMarket.Service.Interfaces.Kassas;

namespace SmartMarket.Service.Services.Kassas;

public class KassaService : IKassaService
{
    private readonly IRepository<Kassa> _kassaRepository;
    private readonly IMapper _mapper;

    public KassaService(IRepository<Kassa> kassaRepository, IMapper mapper)
    {
        _mapper = mapper;
        _kassaRepository = kassaRepository;
    }

    public async Task<KassaForResultDto> CreateAsync(KassaForCreationDto dto)
    {
        var category = await _kassaRepository.SelectAll()
            .Where(c => c.Name.ToLower() == dto.Name.ToLower())
            .FirstOrDefaultAsync();
        if (category is not null)
            throw new CustomException(403, "Bu kassa bazada mavjud.");

        var mappedCategory = _mapper.Map<Kassa>(dto);
        mappedCategory.CreatedAt = DateTime.UtcNow;

        var result = await _kassaRepository.InsertAsync(mappedCategory);

        return _mapper.Map<KassaForResultDto>(result);
    }

    public async Task<KassaForResultDto> ModifyAsync(KassaForUpdateDto dto)
    {
        var category = await _kassaRepository.SelectAll()
            .Where(c => c.Id == dto.Id)
            .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Kassa topilmadi.");

        var mappedCategory = _mapper.Map(dto, category);
        mappedCategory.UpdatedAt = DateTime.UtcNow;

        var result = await _kassaRepository.UpdateAsync(mappedCategory);

        return _mapper.Map<KassaForResultDto>(result);
    }

    public async Task<bool> ReamoveAsync(long id)
    {
        var category = await _kassaRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Kassa topilmadi.");

        await _kassaRepository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<KassaForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var categories = await _kassaRepository.SelectAll()
                .Include(c => c.Tolovs)
                .Include(c => c.Cards)
                .Include(p => p.PartnerProducts)
                .AsNoTracking()
                .ToPagedList(@params)
                .ToListAsync();

        return _mapper.Map<IEnumerable<KassaForResultDto>>(categories);
    }

    public async Task<KassaForResultDto> RetrieveByIdAsync(long id)
    {
        var category = await _kassaRepository.SelectAll()
                .Where(c => c.Id == id)
                .Include(c => c.Tolovs)
                .Include(c => c.Cards)
                .Include(p => p.PartnerProducts)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Kassa topilmadi.");

        return _mapper.Map<KassaForResultDto>(category);
    }
}
