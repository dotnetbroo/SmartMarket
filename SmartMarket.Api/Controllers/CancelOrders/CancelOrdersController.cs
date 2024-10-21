using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.Interfaces.CencelOrders;

namespace SmartMarket.Api.Controllers.CancelOrders
{
    [Authorize]
    public class CancelOrdersController : BaseController
    {
        private readonly ICancelOrderService _cancelOrderService;

        public CancelOrdersController(ICancelOrderService cancelOrderService)
        {
            _cancelOrderService = cancelOrderService;
        }

        [HttpPost("magazinda-sotilgan-mahsulotlar-uchun")]
        public async Task<IActionResult> PostAsync(long id, decimal quantity, long canceledBy, string reason, bool action)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _cancelOrderService.CanceledProductsAsync(id, quantity, canceledBy, reason, action)
           });

        [HttpPost("hamkorlardan-qaytgan-mahsulotlar-uchun")]
        public async Task<IActionResult> PostPartnetCanceledProductsAsync(long id, long partnerId, decimal quantity, long canceledBy, string reason, bool action)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _cancelOrderService.CanceledProductsFromParterAsync(id, partnerId, quantity, canceledBy, reason, action)
           });

        [HttpGet("ikkita-vaqt-orasida-magazindagi-mahsulotlarni-kurish/{userId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetAllAsync(long userId, DateTime startDate, DateTime endDate)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cancelOrderService.RetrieveAllWithDateTimeAsync(userId, startDate, endDate)
            });

        [HttpGet("yaroqsiz-mahsulotlarni-korish/{startDate}/{endDate}")]
        public async Task<IActionResult> GetAllYaroqsizAsync(DateTime startDate, DateTime endDate)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cancelOrderService.YaroqsizMahsulotlarAsync(startDate, endDate)
            });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cancelOrderService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cancelOrderService.RetrieveByIdAsync(id)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cancelOrderService.ReamoveAsync(id)
            });
    }
}
