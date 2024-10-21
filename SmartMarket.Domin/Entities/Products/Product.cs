using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Domin.Entities.Products;

public class Product : Auditable
{
    public string PCode { get; set; }
    public string BarCode { get; set; }
    public string Name { get; set; }
    public long CategoryId { get; set; }
    public Category Category { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public decimal CamePrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public OlchovBirligi OlchovTuri { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? PercentageOfPrice { get; set; }
    public bool Action {  get; set; }
    public string ImagePath { get; set; }
    public long ContrAgentId { get; set; }
    public ContrAgent ContrAgent { get; set; }
}
