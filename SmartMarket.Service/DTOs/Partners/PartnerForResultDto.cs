using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Enums;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Korzinkas;
using SmartMarket.Service.DTOs.Orders;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.Tolov;

namespace SmartMarket.Service.DTOs.Partners;

public record PartnerForResultDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Debt { get; set; }
    public decimal Paid { get; set; }
    public long TolovUsuliId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<KorzinkaForResultDto> Korzinkas { get; set; }
    public ICollection<CardForResultDto> Cards { get; set; }
    public ICollection<OrderForResultDto> Orders { get; set; }
    public ICollection<PartnerTolovForResultDto> PartnerTolovs { get; set; }

    public ICollection<PartnerProductForResultDto> PartnerProducts { get; set; }
}
