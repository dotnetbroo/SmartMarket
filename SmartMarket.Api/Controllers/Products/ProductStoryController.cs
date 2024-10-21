using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.Interfaces.Products;

namespace SmartMarket.Api.Controllers.Products;

public class ProductStoryController : BaseController
{
    private readonly IProductStoryService _productStoryService;

    public ProductStoryController(IProductStoryService productStoryService)
    {
        _productStoryService = productStoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productStoryService.GetAllAsync(@params)
        });


    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productStoryService.GetByIdAsync(id)
        });

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productStoryService.DeleteAsync(id)
        });
}
