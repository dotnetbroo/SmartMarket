using SmartMarket.Domin.Commons;

namespace SmartMarket.Domin.Entities.Partners;

public class PartnerTolov : Auditable
{
    public long PartnerId { get; set; }
    public Partner Partner { get; set; }
    public decimal LastPaid { get; set; }
}
