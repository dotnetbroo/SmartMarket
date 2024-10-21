using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Products;
using SmartMarket.Service.Interfaces.Products;
using System.ComponentModel.DataAnnotations;

namespace SmartMarket.Api.Controllers.Products;
[Authorize]
public class ProductsController : BaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromForm] ProductForCreationDto productForCreationDto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productService.CreateAsync(productForCreationDto)
        });

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productService.GetAllAsync(@params)
        });


    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([Required] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productService.GetByIdAsync(id)
        });

    [HttpPut]
    public async Task<IActionResult> ModifyAsync([FromBody]ProductForUpdateDto productForUpdate)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productService.UpdateAsync(productForUpdate)
        });

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([Required] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _productService.DeleteAsync(id)
        });


}
