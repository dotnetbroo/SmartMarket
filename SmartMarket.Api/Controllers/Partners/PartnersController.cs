using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Partners;
using SmartMarket.Service.Interfaces.Partners;

namespace SmartMarket.Api.Controllers.Partners
{
    [Authorize]
    public class PartnersController : BaseController
    {
        private readonly IPartnerService _partnerService;

        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromForm] PartnerForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await _partnerService.CreateAsync(dto)
        });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerService.RetrieveByIdAsync(id)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerService.RemoveAsync(id)
            });

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] PartnerForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerService.ModifyAsync( dto)
            });
    }
}
