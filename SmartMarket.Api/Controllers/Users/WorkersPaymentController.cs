using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.DTOs.Users.Payments;
using SmartMarket.Service.Interfaces.Users;

namespace SmartMarket.Api.Controllers.Users
{
    [Authorize]
    public class WorkersPaymentController : BaseController
    {
        private readonly IWorkersPaymentService _workersPaymentService;

        public WorkersPaymentController(IWorkersPaymentService workersPaymentService)
        {
            _workersPaymentService = workersPaymentService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] WorkersPaymentForCreationDto dto)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _workersPaymentService.CreateAsync(dto)
           });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _workersPaymentService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _workersPaymentService.RetrieveByIdAsync(id)
            });

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] WorkersPaymentForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _workersPaymentService.ModifyAsync(dto)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _workersPaymentService.RemoveAsync(id)
            });
    }
}
