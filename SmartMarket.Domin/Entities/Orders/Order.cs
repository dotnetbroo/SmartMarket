using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Domin.Entities.Orders;

public class Order : Auditable
{
    public string TransNo { get; set; }
    public string ProductName { get; set; }
    public string Barcode { get; set; } 
    public string PCode { get; set; }
    public long CategoryId { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }  
    public decimal Quantity { get; set; }
    public long YiguvchId { get; set; }
    public User Yiguvchi { get; set; }
    public long YukTaxlovchId { get; set; }
    public User YukTaxlovchi { get; set; }
    public decimal TotalPrice { get; set; }
    public long PartnerId { get; set; }
    public Partner Partner { get; set; }
    public OlchovBirligi OlchovTuri { get; set; }
}
