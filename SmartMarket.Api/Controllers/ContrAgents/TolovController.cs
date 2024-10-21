using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.Interfaces.ContrAgents;
using SmartMarket.Service.Services.ContrAgents;

namespace SmartMarket.Api.Controllers.ContrAgents;
[Authorize]
public class TolovController : BaseController
{
    private readonly ITolovService _tolovService;

    public TolovController(ITolovService tolovService)
    {
        _tolovService = tolovService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
    => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _tolovService.RetrieveAllAsync(@params)
            });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _tolovService.RetrieveByIdAsync(id)
        });

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _tolovService.RemoveAsync(id)
        });

}
