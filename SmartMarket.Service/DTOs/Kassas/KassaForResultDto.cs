using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Orders;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.TolovUsullari;

namespace SmartMarket.Service.DTOs.Kassas;

public class KassaForResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdaedAt { get; set; }

    public IEnumerable<PartnerProductForResultDto> PartnerProducts { get; set; }
    public ICollection<TolovUsuliForResultDto> Tolovs { get; set; }
    public ICollection<CardForResultDto> Cards { get; set; }
}
