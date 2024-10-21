using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.DTOs.Kassas;
using SmartMarket.Service.Interfaces.Kassas;

namespace SmartMarket.Api.Controllers.Kassas
{
    [Authorize]
    public class KassaController : BaseController
    {
        private readonly IKassaService _kassaService;

        public KassaController(IKassaService kassaService)
        {
            _kassaService = kassaService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] KassaForCreationDto dto)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _kassaService.CreateAsync(dto)
           });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _kassaService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _kassaService.RetrieveByIdAsync(id)
            });

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] KassaForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _kassaService.ModifyAsync(dto)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _kassaService.ReamoveAsync(id)
            });
    }
}
