using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Enums;
using SmartMarket.Service.DTOs.Cards;

namespace SmartMarket.Service.Interfaces.Cards;

public interface ICardService
{
    Task<bool> ReamoveAsync(long id);//
    Task<CardForResultDto> RetrieveByIdAsync(long id);//
    Task<IEnumerable<CardForResultDto>> RetrieveAllWithMaxSaledAsync(DateTime startDate, DateTime endDate, int takeMax);//
    Task<IEnumerable<CardForResultDto>> RetrieveAllAsync(PaginationParams @params);//
    Task<IEnumerable<CardForResultDto>> RetrieveAllWithDateTimeAsync(long userId, DateTime startDate, DateTime endDate);//
}
