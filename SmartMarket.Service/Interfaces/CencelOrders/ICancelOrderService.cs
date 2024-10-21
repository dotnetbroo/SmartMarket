using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.CancelOrders;

namespace SmartMarket.Service.Interfaces.CencelOrders;

public interface ICancelOrderService
{
    Task<bool> ReamoveAsync(long id);
    Task<CancelOrderForResultDto> RetrieveByIdAsync(long id);
    Task<IEnumerable<CancelOrderForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<IEnumerable<CancelOrderForResultDto>> YaroqsizMahsulotlarAsync(DateTime startDate, DateTime endDate);
    Task<CancelOrderForResultDto> CanceledProductsAsync(long id, decimal quantity, long canceledBy, string reason, bool action);
    Task<IEnumerable<CancelOrderForResultDto>> RetrieveAllWithDateTimeAsync(long userId, DateTime startDate, DateTime endDate);
    Task<CancelOrderForResultDto> CanceledProductsFromParterAsync(long id, long partnerId, decimal quantity, long canceledBy, string reason, bool action);
}
