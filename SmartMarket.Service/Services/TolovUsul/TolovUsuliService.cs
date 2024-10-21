using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.TolovUsullari;
using SmartMarket.Service.Interfaces.TolovUsuli;

namespace SmartMarket.Service.Services.TolovUsul;

public class TolovUsuliService : ITolovUsuliService
{
    private readonly IRepository<TolovUsuli> _tolovRepository;
    private readonly IRepository<PartnerTolov> _partnerRepository;
    private readonly IMapper _mapper;

    public TolovUsuliService(IRepository<TolovUsuli> tolovRepository, IMapper mapper, IRepository<PartnerTolov> partnerRepository)
    {
        _mapper = mapper;
        _tolovRepository = tolovRepository;
        _partnerRepository = partnerRepository;
    }

    public async Task<TolovUsuliForResultDto> CreateAsync(TolovUsuliForCreationDto dto)
    {
        var mapped = _mapper.Map<TolovUsuli>(dto);
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _tolovRepository.InsertAsync(mapped);

        return _mapper.Map<TolovUsuliForResultDto>(result);
    }


    public async Task<IEnumerable<TolovUsuliForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var categories = await _tolovRepository.SelectAll()
                .AsNoTracking()
                .ToPagedList(@params)
                .ToListAsync();

        return _mapper.Map<IEnumerable<TolovUsuliForResultDto>>(categories);
    }

    public async Task<TolovUsuliForResultDto> RetrieveByIdAsync(long id)
    {
        var category = await _tolovRepository.SelectAll()
                .Where(c => c.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Tolov usuli topilmadi.");

        return _mapper.Map<TolovUsuliForResultDto>(category);
    }

    public async Task<HisobotForResultDto> GetNaqtTolovHisoboti(long kassaId, DateTime startDate, DateTime endDate)
    {
        var naqdTolovs = await _tolovRepository.SelectAll()
            .Where(n => n.CreatedAt >= startDate && n.CreatedAt <= endDate && n.Status == "Tolanmagan")
            .AsNoTracking()
            .ToListAsync();

        var totalNaqt = 0m;
        var totalKarta = 0m;
        var totalPulKochirish = 0m;
        var nasiya = 0m;
        var tolanganNasiya = 0m;

        foreach (var naqd in naqdTolovs)
        {
            totalNaqt += naqd.Naqt ?? 0;
            totalKarta += naqd.Karta ?? 0;
            totalPulKochirish += naqd.PulKochirish ?? 0;
            nasiya += naqd.Nasiya ?? 0;
        }

        var partner = await _partnerRepository.SelectAll()
            .Where(n => n.CreatedAt >= startDate && n.CreatedAt <= endDate)
            .AsNoTracking()
            .ToListAsync();

        foreach(var tolangan in partner)
        {
            tolanganNasiya += tolangan.LastPaid;
        }

        return new HisobotForResultDto()
        {
            KassaId = kassaId,
            Naqt = totalNaqt,
            Karta = totalKarta,
            PulKochirish = totalPulKochirish,
            Nasiya = nasiya,
            TolanganNasiya = tolanganNasiya
        };
    }
}
