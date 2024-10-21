using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Service.DTOs.Cards;

public record CardForResultDto
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
    public long CasherId { get; set; }
    public long? YukTaxlovchId { get; set; }
    public long PartnerId { get; set; }
    public long KassaId { get; set; }
    public string Status { get; set; }
    public OlchovBirligi OlchovBirligi { get; set; }
    public long TolovUsulId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
