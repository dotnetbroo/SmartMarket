using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.Partners;
using SmartMarket.Service.Interfaces.PartnerProducts;

namespace SmartMarket.Service.Services.PartnerProducts;

public class PartnerProductService : IPartnerProductService
{
    private readonly IRepository<TolovUsuli> _tolovRepository;
    private readonly IRepository<Partner> _partnerRepository;
    private readonly IRepository<PartnerProduct> _partnerProductRepository;
    private readonly IRepository<PartnerTolov> _partnerTolovRepository;
    private readonly IMapper _mapper;

    public PartnerProductService(
        IRepository<PartnerProduct> partnerProductRepository,
        IMapper mapper,
        IRepository<PartnerTolov> partnerTolovRepository,
        IRepository<Partner> partnerRepository,
        IRepository<TolovUsuli> tolovRepository)
    {
        _partnerProductRepository = partnerProductRepository;
        _mapper = mapper;
        _partnerTolovRepository = partnerTolovRepository;
        _partnerRepository = partnerRepository;
        _tolovRepository = tolovRepository;
    }

    public async Task<PartnerForResultDto> PayForProductsAsync(long partnerId, decimal paid, long tolovUsuli)
    {
        var partnerProduct = await _partnerProductRepository.SelectAll()
            .Where(p => p.PartnerId == partnerId)
            .FirstOrDefaultAsync();
        if (partnerProduct is not null)
        {
            var partnerDebt = await _partnerRepository.SelectAll()
            .Where(p => p.Id == partnerProduct.PartnerId)
            .FirstOrDefaultAsync();
            if (partnerDebt.Debt > 0)
            {
                var nat = partnerDebt.Debt -= paid;
                partnerDebt.Debt = nat;
                partnerDebt.Paid = paid;
                partnerDebt.TolovUsuliId = tolovUsuli;
                partnerDebt.UpdatedAt = DateTime.UtcNow;

                var tolov = new PartnerTolov
                {
                    PartnerId = partnerDebt.Id,
                    LastPaid = partnerDebt.Paid,
                    CreatedAt = DateTime.UtcNow
                };

                await _partnerTolovRepository.InsertAsync(tolov);

                await _partnerRepository.UpdateAsync(partnerDebt);

                var tolovs = await _tolovRepository.SelectAll()
                    .Where(t => t.Id == tolovUsuli)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (tolovs != null)
                {
                    tolovs.Status = partnerDebt.Debt == 0 ? "Tolangan" : "Tolanmagan";
                    await _tolovRepository.UpdateAsync(tolovs);
                }
            }
            else
            {
                throw new CustomException(400, "Qarz qolmadi.");
            }
        }
        return _mapper.Map<PartnerForResultDto>(partnerProduct);
    }


    public async Task<bool> RemoveAsync(long id)
    {
        var StockProduct = await _partnerProductRepository.SelectAll()
            .Where(s => s.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (StockProduct is null)
            throw new CustomException(404, "Bu mahsulot topilmadi.");

        return await _partnerProductRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<PartnerProductForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var partnerProducts = await _partnerProductRepository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PartnerProductForResultDto>>(partnerProducts);
    }

    public async Task<IEnumerable<PartnerProductForResultDto>> RetrieveAllWithDateTimeAsync(long userId, DateTime startDate, DateTime endDate)
    {
        if (userId != null)
        {
            var product = await _partnerProductRepository.SelectAll()
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        var products = await _partnerProductRepository.SelectAll()
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<IEnumerable<PartnerProductForResultDto>>(products);
    }

    public async Task<PartnerProductForResultDto> RetrieveByIdAsync(long id)
    {
        var StockProduct = await _partnerProductRepository.SelectAll()
          .Where(s => s.Id == id)
          .AsNoTracking()
          .FirstOrDefaultAsync();

        if (StockProduct is null)
            throw new CustomException(404, "Bu mahsulot topilmadi.");

        return _mapper.Map<PartnerProductForResultDto>(StockProduct);
    }

    public async Task<PartnerProductForResultDto> RetrieveByTransNoAsync(string transNo)
    {
        var partnerProduct = await _partnerProductRepository.SelectAll()
          .Where(s => s.TransNo == transNo)
          .AsNoTracking()
          .FirstOrDefaultAsync();

        if (partnerProduct is null)
            throw new CustomException(404, "Bu mahsulot topilmadi.");

        return _mapper.Map<PartnerProductForResultDto>(partnerProduct);
    }
}
