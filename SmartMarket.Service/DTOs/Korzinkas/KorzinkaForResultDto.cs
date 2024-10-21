using SmartMarket.Domin.Enums;

namespace SmartMarket.Service.DTOs.Korzinkas;

public record KorzinkaForResultDto
{
    public long Id { get; set; }
    public string TransNo { get; set; }
    public string ProductName { get; set; }
    public string BarCode { get; set; }
    public string PCode { get; set; }
    public long CategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public long? YukYiguvchId { get; set; }
    public long? YukTaxlovchId { get; set; }
    public long? PartnerId { get; set; }
    public OlchovBirligi OlchovBirligi { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
