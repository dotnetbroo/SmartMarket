using AutoMapper;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.CencelOrders;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Service.DTOs.CancelOrders;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.DTOs.ContrAgents;
using SmartMarket.Service.DTOs.Kassas;
using SmartMarket.Service.DTOs.Korzinkas;
using SmartMarket.Service.DTOs.Orders;
using SmartMarket.Service.DTOs.PartnerProducts;
using SmartMarket.Service.DTOs.Partners;
using SmartMarket.Service.DTOs.Products;
using SmartMarket.Service.DTOs.Tolov;
using SmartMarket.Service.DTOs.TolovUsullari;
using SmartMarket.Service.DTOs.Users;
using SmartMarket.Service.DTOs.Users.Payments;
using System.Security.Cryptography.X509Certificates;

namespace SmartMarket.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Users
        CreateMap<User, UserForResultDto>().ReverseMap();
        CreateMap<User, UserForUpdateDto>().ReverseMap();
        CreateMap<User, UserForCreationDto>().ReverseMap();
        CreateMap<User, UserForChangePasswordDto>().ReverseMap();

        // Category
        CreateMap<Category, CategoryForResultDto>().ReverseMap();
        CreateMap<Category, CategoryForUpdateDto>().ReverseMap();
        CreateMap<Category, CategoryForCreationDto>().ReverseMap();

        // Cards
        CreateMap<Card, CardForResultDto>().ReverseMap();

        // CancelOrder
        CreateMap<CencelOrder, CancelOrderForResultDto>().ReverseMap();

        //Products
        CreateMap<Product, ProductForResultDto>().ReverseMap();
        CreateMap<Product, ProductForUpdateDto>().ReverseMap();
        CreateMap<Product, ProductForCreationDto>().ReverseMap();
        CreateMap<ProductStory, ProductStoryForResultDto>().ReverseMap();

        // Partner
        CreateMap<Partner, PartnerForCreationDto>().ReverseMap();
        CreateMap<Partner, PartnerForResultDto>().ReverseMap();
        CreateMap<Partner, PartnerForUpdateDto>().ReverseMap();
        CreateMap<PartnerTolov, PartnerTolovForResultDto>().ReverseMap();

        // PartnerProduct
        CreateMap<PartnerProduct, PartnerProductForResultDto>().ReverseMap();

        // ContrAgent
        CreateMap<ContrAgent, ContrAgentForResultDto>().ReverseMap();
        CreateMap<ContrAgent, ContrAgentForUpdateDto>().ReverseMap();
        CreateMap<ContrAgent, ContrAgentForCreationDto>().ReverseMap();

        // Tolov
        CreateMap<Tolov, TolovForResultDto>().ReverseMap();

        // Kassa
        CreateMap<Kassa, KassaForCreationDto>().ReverseMap();
        CreateMap<Kassa, KassaForResultDto>().ReverseMap();
        CreateMap<Kassa, KassaForUpdateDto>().ReverseMap();

        //Order
        CreateMap<Order, OrderForResultDto>().ReverseMap();
        CreateMap<Order, OrderForUpdateDto>().ReverseMap();

        // Korzinka 
        CreateMap<Korzinka, KorzinkaForResultDto>().ReverseMap();
        CreateMap<Korzinka, KorzinkaForUpdateDto>().ReverseMap();

        // WorkersPayment
        CreateMap<WorkersPayment, WorkersPaymentForCreationDto>().ReverseMap();
        CreateMap<WorkersPayment,WorkersPaymentForResultDto>().ReverseMap();
        CreateMap<WorkersPayment, WorkersPaymentForUpdateDto>().ReverseMap();

        //TolovUsuli
        CreateMap<TolovUsuli, TolovUsuliForCreationDto>().ReverseMap();
        CreateMap<TolovUsuli, TolovUsuliForResultDto>().ReverseMap();
    }
}
