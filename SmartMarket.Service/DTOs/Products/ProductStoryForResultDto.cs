using SmartMarket.Domin.Enums;

namespace SmartMarket.Service.DTOs.Products;

public class ProductStoryForResultDto
{
    public long Id { get; set; }
    public string PCode { get; set; }
    public string BarCode { get; set; }
    public string Name { get; set; }
    public long CategoryId { get; set; }
    public long UserId { get; set; }
    public decimal CamePrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public OlchovBirligi OlchovTuri { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? PercentageOfPrice { get; set; }
    public bool Action { get; set; }
    public long ContrAgentId { get; set; }
    public string ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
