using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.Interfaces.Orders;

namespace SmartMarket.Api.Controllers.Orders
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("transaction-generator")]
        public async Task<IActionResult> GenerateTranNo()
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await Task.FromResult(_orderService.GenerateTransactionNumber())
           });

        [HttpPost("id-bilan-sotish")]
        public async Task<IActionResult> PostAsync(long id, long yukTaxlovchId, long yukYiguvchId, long partnerId, decimal quantityToMove, string transNo)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _orderService.MoveProductToOrderAsync(id, yukYiguvchId, yukTaxlovchId, partnerId, quantityToMove, transNo)
           });

        [HttpPost("korzinkadan-yukni-qaytarish")]
        public async Task<IActionResult> YukniQaytarish(long id, string barCode, decimal quantity)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _orderService.CanceledOrderByPlanshetAsync(id, barCode, quantity)
           });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _orderService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _orderService.RetrieveByIdAsync(id)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _orderService.ReamoveAsync(id)
            });
    }
}
