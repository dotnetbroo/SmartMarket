using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.ContrAgents;
using SmartMarket.Service.DTOs.Partners;
using SmartMarket.Service.Interfaces.ContrAgents;

namespace SmartMarket.Api.Controllers.ContrAgents
{
    [Authorize]
    public class ContrAgentsController : BaseController
    {
        private readonly IContrAgentService _contrAgentService;

        public ContrAgentsController(IContrAgentService contrAgentService)
        {
            _contrAgentService = contrAgentService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromForm] ContrAgentForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _contrAgentService.CreateAsync(dto)
        });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _contrAgentService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _contrAgentService.RetrieveByIdAsync(id)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _contrAgentService.RemoveAsync(id)
            });

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ContrAgentForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _contrAgentService.ModifyAsync(dto)
            });

        [HttpPut("pay-for-agent's-product")]
        public async Task<IActionResult> UpdatePaidAsync(long partnerId, decimal paid, long tolovUsulID)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _contrAgentService.PayForProductsAsync(partnerId, paid, tolovUsulID)
            });
    }
}
