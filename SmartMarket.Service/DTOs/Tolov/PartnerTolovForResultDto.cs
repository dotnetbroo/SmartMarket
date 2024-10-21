namespace SmartMarket.Service.DTOs.Tolov;

public record PartnerTolovForResultDto
{
    public long Id { get; set; }
    public long PartnerId { get; set; }
    public decimal LastPaid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
