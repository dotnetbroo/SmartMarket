namespace SmartMarket.Service.DTOs.TolovUsullari;

public record TolovUsuliForCreationDto
{
    public long KassaId { get; set; }
    public decimal? Naqt { get; set; }
    public decimal? Karta { get; set; }
    public decimal? PulKochirish { get; set; }
}
