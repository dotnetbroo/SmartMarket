using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.Interfaces.Cards;

namespace SmartMarket.Service.Services.Cards;

public class CardService : ICardService
{
    private readonly IRepository<Card> _cardRepository;
    private readonly IMapper _mapper;

    public CardService(IRepository<Card> cardRepository, IMapper mapper)
    {
        _cardRepository = cardRepository;
        _mapper = mapper;
    }

    public async Task<bool> ReamoveAsync(long id)
    {
        var userlanguage = await _cardRepository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
        if (userlanguage is null)
            throw new CustomException(404, "Karta topilmadi.");

        await _cardRepository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<CardForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var cards = await _cardRepository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CardForResultDto>>(cards);
    }

    public async Task<CardForResultDto> RetrieveByIdAsync(long id)
    {
        var card = await _cardRepository.SelectAll()
           .Where(c => c.Id == id)
           .AsNoTracking()
           .FirstOrDefaultAsync();
        if (card is null)
            throw new CustomException(404, "Karta topilmadi.");

        return _mapper.Map<CardForResultDto>(card);
    }

    public async Task<IEnumerable<CardForResultDto>> RetrieveAllWithDateTimeAsync(long userId, DateTime startDate, DateTime endDate)
    {
        if (userId != null)
        {
            var product = await _cardRepository.SelectAll()
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.CasherId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        var products = await _cardRepository.SelectAll()
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<IEnumerable<CardForResultDto>>(products);
    }

    public async Task<IEnumerable<CardForResultDto>> RetrieveAllWithMaxSaledAsync(DateTime startDate, DateTime endDate, int takeMax)
    {
        var result = await _cardRepository.SelectAll()
            .Where(c => c.Status == "Sotildi" && c.CreatedAt >= startDate && c.CreatedAt <= endDate)
            .OrderByDescending(c => c.Quantity)
            .Take(takeMax)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<IEnumerable<CardForResultDto>>(result);
    }
}
