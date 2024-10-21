using SmartMarket.Domin.Enums;

namespace SmartMarket.Service.DTOs.Orders;

public record OrderForResultDto
{
    public long Id { get; set; }
    public string TransNo { get; set; }
    public string ProductName { get; set; }
    public string Barcode { get; set; }
    public string PCode { get; set; }
    public long CategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public long YiguvchId { get; set; }
    public long YukTaxlovchId { get; set; }
    public decimal TotalPrice { get; set; }
    public long PartnerId { get; set; }
    public OlchovBirligi OlchovTuri { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
