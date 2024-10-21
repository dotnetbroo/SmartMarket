using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Domin.Entities.Cards;

public class Card : Auditable
{
    public string TransNo { get; set; }
    public string ProductName { get; set; }
    public string PCode { get; set; }
    public string BarCode { get; set; }
    public long CategoryId { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public long? YukYiguvchId { get; set; }
    public User Yiguvchi { get; set; }
    public long CasherId { get; set; }
    public User Casher { get; set; }
    public long? YukTaxlovchId { get; set; }
    public User YukTaxlovchi { get; set; }
    public long KassaId { get; set; }
    public Kassa Kassa { get; set; }
    public string Status { get; set; }
    public OlchovBirligi OlchovBirligi { get; set; }
    public long TolovUsulId { get; set; }
    public TolovUsuli TolovUsuli { get; set; }
    public long? PartnerId { get; set; }
    public Partner Partner { get; set; }
}
