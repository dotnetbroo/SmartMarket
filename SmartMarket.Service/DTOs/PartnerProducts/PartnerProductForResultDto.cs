using SmartMarket.Domin.Entities.Users;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Service.DTOs.PartnerProducts;

public record PartnerProductForResultDto
{
    public long Id { get; set; }
    public long? PartnerId { get; set; }
    public long CategoryId { get; set; }
    public long UserId { get; set; }
    public long? YukYiguvchId { get; set; }
    public long? YukTaxlovchId { get; set; }
    public string ProductName { get; set; }
    public string TransNo { get; set; }
    public string PCode { get; set; }
    public string BarCode { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal DiscountPrice { get; set; }
    public OlchovBirligi OlchovBirligi { get; set; }
    public string Status { get; set; }
    public long KassaId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
