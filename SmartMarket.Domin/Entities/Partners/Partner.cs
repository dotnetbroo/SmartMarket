using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Domin.Entities.Partners;

public class Partner : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Debt { get; set; }
    public decimal Paid { get; set; }
    public long TolovUsuliId { get; set; }
    public TolovUsuli TolovUsuli { get; set; }

    public IEnumerable<Korzinka> Korzinkas { get; set; }
    public IEnumerable<PartnerTolov> PartnerTolovs { get; set; }
    public IEnumerable<Card> Cards { get; set; }
    public IEnumerable<Order> Orders { get; set; }
    public IEnumerable<PartnerProduct> PartnerProducts { get; set; }
}
