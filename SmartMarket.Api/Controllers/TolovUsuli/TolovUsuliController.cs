using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.TolovUsullari;
using SmartMarket.Service.Interfaces.TolovUsuli;

namespace SmartMarket.Api.Controllers.TolovUsuli
{
    [Authorize]
    public class TolovUsuliController : BaseController
    {
        private readonly ITolovUsuliService _tolovUsuliService;

        public TolovUsuliController(ITolovUsuliService tolovUsuliService)
        {
            _tolovUsuliService = tolovUsuliService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TolovUsuliForCreationDto dto)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _tolovUsuliService.CreateAsync(dto)
           });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _tolovUsuliService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _tolovUsuliService.RetrieveByIdAsync(id)
            });

        [HttpGet("Hisobot")]
        public async Task<IActionResult> GetHisobotAsync(long kassaId, DateTime startDate, DateTime endDate)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _tolovUsuliService.GetNaqtTolovHisoboti(kassaId, startDate, endDate)
            });
    }
}
