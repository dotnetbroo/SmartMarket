using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Domin.Entities.Partners;

namespace SmartMarket.Domin.Entities.Tolovs;

public class TolovUsuli : Auditable
{
    public long KassaId { get; set; }
    public Kassa Kassa { get; set; }
    public decimal? Naqt { get; set; }
    public decimal? Karta { get; set; }
    public decimal? PulKochirish { get; set; }
    public decimal? Nasiya { get; set; }
    public string Status { get; set; }

    public IEnumerable<ContrAgent> ContrAgents { get; set; }
    public IEnumerable<Korzinka> Korzinkas { get; set; }
    public IEnumerable<Partner> Partners { get; set; }
    public IEnumerable<Card> Cards { get; set; }
    public IEnumerable<PartnerProduct> PartnersProduct { get; set; }
}
