using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Domin.Entities.CencelOrders;

public class CencelOrder : Auditable
{
    public string TransNo { get; set; }
    public string ProductName { get; set; }
    public string BarCode { get; set; }
    public long CategoryId { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public OlchovBirligi OlchovTuri { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public long CasherId { get; set; }
    public User Casher { get; set; }
    public string Reason { get; set; }
    public long CancelerCasherId { get; set; }
    public User CencelerCasher { get; set; }
    public DateTime ReturnDate { get; set; }
    public bool Action {  get; set; }
    public string Status { get; set; }

}
