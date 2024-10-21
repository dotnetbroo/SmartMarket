using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Orders;

namespace SmartMarket.Service.Interfaces.Orders;

public interface IOrderService
{
    Task<bool> ReamoveAsync(long id);// 
    string GenerateTransactionNumber();//
    Task<OrderForResultDto> RetrieveByIdAsync(long id);//
    Task<IEnumerable<OrderForResultDto>> RetrieveAllAsync(PaginationParams @params);//
    Task<bool> CanceledOrderByPlanshetAsync(long id, string barCode, decimal quantity);//
    Task<OrderForResultDto> MoveProductToOrderAsync(long id, long yukTaxlovchId, long yukYiguvchId, long partnerId, decimal quantityToMove, string transNo);//
}
