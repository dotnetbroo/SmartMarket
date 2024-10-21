using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.Partners;
using SmartMarket.Service.Interfaces.Partners;

namespace SmartMarket.Service.Services.Partners;

public class PartnerService : IPartnerService
{
    private readonly IRepository<Partner> _partnerRepository;
    private readonly IMapper _mapper;

    public PartnerService(IRepository<Partner> partnerRepository, IMapper mapper)
    {
        _partnerRepository = partnerRepository;
        _mapper = mapper;
    }

    public async Task<PartnerForResultDto> CreateAsync(PartnerForCreationDto dto)
    {
        var user = await _partnerRepository.SelectAll()
            .Where(u => u.PhoneNumber == dto.PhoneNumber)
            .FirstOrDefaultAsync();

        if (user is not null)
            throw new CustomException(403, "Bu hamkorimiz mavjud.");

        var mapped = _mapper.Map<Partner>(dto);
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _partnerRepository.InsertAsync(mapped);

        return _mapper.Map<PartnerForResultDto>(result);
    }

    public async Task<PartnerForResultDto> ModifyAsync(PartnerForUpdateDto dto)
    {
        var user = await _partnerRepository.SelectAll()
             .Where(u => u.Id == dto.Id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Hamkor topilmadi.");

        var mapped = _mapper.Map(dto, user);
        mapped.UpdatedAt = DateTime.UtcNow;

        await _partnerRepository.UpdateAsync(mapped);

        return _mapper.Map<PartnerForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var user = await _partnerRepository.SelectAll()
              .Where(u => u.Id == id)
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Hamkor topilmadi.");

        await _partnerRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<PartnerForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await _partnerRepository.SelectAll()
             .Include(u => u.PartnerProducts)
             .AsNoTracking()
             .ToPagedList(@params)
             .ToListAsync();

        return _mapper.Map<IEnumerable<PartnerForResultDto>>(users);
    }

    public async Task<PartnerForResultDto> RetrieveByIdAsync(long id)
    {
        var user = await _partnerRepository.SelectAll()
             .Where(u => u.Id == id)
             .Include(u => u.PartnerProducts)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Hamkor topilmadi.");

        return _mapper.Map<PartnerForResultDto>(user);
    }
}
