using SmartMarket.Domin.Commons;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.CencelOrders;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Domin.Enums;

namespace SmartMarket.Domin.Entities.Users;

public class User : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public bool IsActive { get; set; }
    public UserRole Role { get; set; }

    public IEnumerable<WorkersPayment> Payments { get; set; }

    public IEnumerable<Order> YukYiguvchi { get; set; }
    public IEnumerable<Order> YukTaxlovchi { get; set; }

    public IEnumerable<Korzinka> Yiguvchi { get; set; }
    public IEnumerable<Korzinka> Taxlovchi { get; set; }

    public IEnumerable<Card> YiguvchiCards { get; set; }
    public IEnumerable<Card> CasherCards { get; set; }
    public IEnumerable<Card> YukTaxlovchisi { get; set; }

    public IEnumerable<CencelOrder> CencelOrders { get; set; }

    public IEnumerable<ProductStory> ProductStory { get; set; }

    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<PartnerProduct> PartnerProducts { get; set; }
    public IEnumerable<PartnerProduct> PartnersYukYiguvchi { get; set; }
    public IEnumerable<PartnerProduct> PartnersYukTaxlovchisi { get; set; }
    public IEnumerable<TolovUsuli> TolovUsulis { get; set; }
}

