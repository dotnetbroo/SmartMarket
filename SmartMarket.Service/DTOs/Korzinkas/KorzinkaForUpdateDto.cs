namespace SmartMarket.Service.DTOs.Korzinkas;

public record KorzinkaForUpdateDto
{
    public long Id { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal Quantity { get; set; }
}
