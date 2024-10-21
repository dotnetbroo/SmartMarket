using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.Interfaces.Cards;

namespace SmartMarket.Api.Controllers.Cards
{
    [Authorize]
    public class CardsController : BaseController
    {
        private readonly ICardService _cardService;
        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cardService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cardService.RetrieveByIdAsync(id)
            });


        [HttpGet("tarixni-korish/{userId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetAllCardProductsAsync(long userId, DateTime startDate, DateTime endDate)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cardService.RetrieveAllWithDateTimeAsync(userId, startDate, endDate)
            });

        [HttpGet("kop-sotilgan-yuk/{max}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetByMaxAsync(DateTime startDate, DateTime endDate, int max)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cardService.RetrieveAllWithMaxSaledAsync(startDate, endDate, max)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _cardService.ReamoveAsync(id)
            });
    }
}
