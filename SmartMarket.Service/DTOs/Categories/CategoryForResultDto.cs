using SmartMarket.Service.DTOs.CancelOrders;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Orders;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.Products;

namespace SmartMarket.Service.DTOs.Categories;

public record CategoryForResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<CardForResultDto> Cards { get; set; }
    public ICollection<ProductForResultDto> Products { get; set; }
    public ICollection<PartnerProductForResultDto> PartnersProducts { get; set; }
    public ICollection<CancelOrderForResultDto> CencelOrders { get; set; }
    public ICollection<OrderForResultDto> Orders { get; set; }
}