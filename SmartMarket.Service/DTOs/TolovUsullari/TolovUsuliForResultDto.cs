using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Korzinkas;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.Partners;

namespace SmartMarket.Service.DTOs.TolovUsullari;

public record TolovUsuliForResultDto
{
    public long Id { get; set; }
    public long KassaId { get; set; }
    public decimal? Naqt { get; set; }
    public decimal? Karta { get; set; }
    public decimal? PulKochirish { get; set; }
    public decimal? Nasiya { get; set; }
    public string Status { get; set; }


    public ICollection<KorzinkaForResultDto> Korzinkas { get; set; }
    public ICollection<PartnerForResultDto> Partners { get; set; }
    public ICollection<CardForResultDto> Cards { get; set; }
    public ICollection<PartnerProductForResultDto> PartnersProduct { get; set; }
}
