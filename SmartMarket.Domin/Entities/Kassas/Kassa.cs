using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Tolovs;

namespace SmartMarket.Domin.Entities.Kassas;

public class Kassa : Auditable
{
    public string Name { get; set; }

    public IEnumerable<PartnerProduct> PartnerProducts { get; set; }
    public IEnumerable<TolovUsuli> Tolovs { get; set; }
    public IEnumerable<Card> Cards { get; set; }
}
