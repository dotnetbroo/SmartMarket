namespace SmartMarket.Service.DTOs.Tolov;

public record TolovForResultDto
{
    public long Id { get; set; }
    public long ContrAgentId { get; set; }
    public decimal LastPaid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}