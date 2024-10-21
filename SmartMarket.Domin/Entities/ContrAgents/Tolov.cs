using SmartMarket.Domin.Commons;

namespace SmartMarket.Domin.Entities.ContrAgents;

public class Tolov : Auditable
{
    public long ContrAgentId { get; set; }
    public ContrAgent ContrAgent { get; set; }
    public decimal LastPaid { get; set; }
}
