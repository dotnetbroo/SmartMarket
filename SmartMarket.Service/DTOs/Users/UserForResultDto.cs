using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.CencelOrders;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Domin.Enums;
using SmartMarket.Service.DTOs.CancelOrders;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Korzinkas;
using SmartMarket.Service.DTOs.Orders;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.Products;
using SmartMarket.Service.DTOs.Users.Payments;

namespace SmartMarket.Service.DTOs.Users;

public record UserForResultDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<OrderForResultDto> YukYiguvchi { get; set; }
    public ICollection<OrderForResultDto> YukTaxlovchi { get; set; }

    public ICollection<KorzinkaForResultDto> Yiguvchi { get; set; }
    public ICollection<KorzinkaForResultDto> Taxlovchi { get; set; }

    public ICollection<CardForResultDto> YiguvchiCards { get; set; }
    public ICollection<CardForResultDto> CasherCards { get; set; }
    public ICollection<CardForResultDto> YukTaxlovchisi { get; set; }

    public ICollection<ProductForResultDto> Products { get; set; }
    public ICollection<PartnerProductForResultDto> PartnerProducts { get; set; }
    public ICollection<PartnerProductForResultDto> PartnersYukYiguvchi { get; set; }
    public ICollection<PartnerProductForResultDto> PartnersYukTaxlovchisi { get; set; }

    public ICollection<WorkersPaymentForResultDto> Payments { get; set; }

    public ICollection<CancelOrderForResultDto> CencelOrders { get; set; }

}
