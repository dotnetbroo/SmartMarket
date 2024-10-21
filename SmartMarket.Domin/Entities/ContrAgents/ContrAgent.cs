using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;

namespace SmartMarket.Domin.Entities.ContrAgents;

public class ContrAgent : Auditable
{
    public string Firma { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Dept { get; set; }
    public decimal PayForDept { get; set; }
    public decimal LastPaid { get; set; }
    public long? TolovUsuliID { get; set; }
    public TolovUsuli TolovUsuli { get; set; }

    public IEnumerable<Tolov> Tolovs { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<ProductStory> ProductStory { get; set; }
}
