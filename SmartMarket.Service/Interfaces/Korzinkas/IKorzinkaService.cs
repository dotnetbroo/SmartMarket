using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Korzinkas;

namespace SmartMarket.Service.Interfaces.Korzinkas;

public interface IKorzinkaService
{
    Task<bool> ReamoveAsync(long id);//
    string GenerateTransactionNumber();//
    Task<KorzinkaForResultDto> RetrieveByIdAsync(long id);//
    Task<KorzinkaForResultDto> GetByBarCodeAsync(string barCode);//
    Task<IEnumerable<KorzinkaForResultDto>> SvetUchgandaAsync(string status);//
    Task<KorzinkaForResultDto> MoveProductsFromOrderToKorzinkaAsync(string transNo);//
    Task<bool> CanceledOrderByKorzinkaAsync(long id, string barCode, decimal quantity);//
    Task<IEnumerable<KorzinkaForResultDto>> RetrieveAllAsync(PaginationParams @params);//
    Task<KorzinkaForResultDto> CalculeteDiscountPercentageAsync(long id, decimal discountPercentage);//
    Task<KorzinkaForResultDto> NasiyaSavdoAsync(string transactionNumber, long partnerId,    long kassaId, long tolovUsuli, long sotuvchiId);//
    Task<KorzinkaForResultDto> UpdateWithTransactionNumberAsync(string transactionNumber, long kassaId, long tolovUsuli, long sotuvchiId);//
    Task<KorzinkaForResultDto> MoveProductToCardAsync(long id, long? yukYiguvchId, long? yukTaxlovchi, long? partnerId, decimal quantityToMove, string transNo);//
    Task<KorzinkaForResultDto> SaleProductWithBarCodeAsync(string barCode, long? yukYiguvchId, long? yukTaxlovchi, long? partnerId, decimal quantityToMove, string transNo);//
}
