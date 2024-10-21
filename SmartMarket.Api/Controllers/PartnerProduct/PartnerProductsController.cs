using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.Interfaces.PartnerProducts;

namespace SmartMarket.Api.Controllers.PartnerProduct
{
    [Authorize]
    public class PartnerProductsController : BaseController
    {
        private readonly IPartnerProductService _partnerProductService;

        public PartnerProductsController(IPartnerProductService partnerProductService)
        {
            _partnerProductService = partnerProductService;
        }

        [HttpPut("pay-for-product")]
        public async Task<IActionResult> UpdatePaidAsync(long partnerId, decimal paid, long tolovUsuli)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerProductService.PayForProductsAsync(partnerId, paid, tolovUsuli)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerProductService.RetrieveByIdAsync(id)
            });

        [HttpGet]
        public async Task<IActionResult> GeAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerProductService.RetrieveAllAsync(@params)
            });

        [HttpGet("get-by-transaction-number")]
        public async Task<IActionResult> GetByTransNo(string transNo)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerProductService.RetrieveByTransNoAsync(transNo)
            });


        [HttpGet("yuk-tarixini-korish/{userId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetAllAsync(long userId, DateTime startDate, DateTime endDate)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerProductService.RetrieveAllWithDateTimeAsync(userId, startDate, endDate)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _partnerProductService.RemoveAsync(id)
            });
    }
}
