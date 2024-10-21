using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Service.Interfaces.Korzinkas;

namespace SmartMarket.Api.Controllers.Cards
{
    [Authorize]
    public class KorzinkasController : BaseController
    {
        private readonly IKorzinkaService _korzinkaService;

        public KorzinkasController(IKorzinkaService korzinkaService)
        {
            _korzinkaService = korzinkaService;
        }

        [HttpGet("transaction-generator")]
        public async Task<IActionResult> GenerateTranNo()
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await Task.FromResult(_korzinkaService.GenerateTransactionNumber())
            });

        [HttpPost("orderdan-korzinkaga-olish")]
        public async Task<IActionResult> OrderdanKorzinkagaOlish(string transNo)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _korzinkaService.MoveProductsFromOrderToKorzinkaAsync(transNo)
           });

        [HttpPost("id-bilan-sotish")]
        public async Task<IActionResult> PostAsync(long id, long? yukYiguvchId, long? yukTaxlovchi, long? partnerId, decimal quantityToMove, string transNo)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _korzinkaService.MoveProductToCardAsync(id, yukYiguvchId, yukTaxlovchi, partnerId,  quantityToMove, transNo)
           });

        [HttpPost("barcode-bilan-sotish")]
        public async Task<IActionResult> SaleAsync(string barCode, long? yukYiguvchId, long? yukTaxlovchi, long? partnerId, decimal quantityToMove, string transNo)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _korzinkaService.SaleProductWithBarCodeAsync(barCode, yukYiguvchId, yukTaxlovchi, partnerId, quantityToMove, transNo)
           });


        [HttpPost("chegirma-berish/{id}/{discountPercentage}")]
        public async Task<IActionResult> CalculateAsync(long id, decimal discountPercentage)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _korzinkaService.CalculeteDiscountPercentageAsync(id, discountPercentage)
           });

        [HttpPut("{transactionNumber}")]
        public async Task<IActionResult> Updateasync([FromRoute(Name = "transactionNumber")] string transactionNumber, long kassaId, long tolovUsuli, long sotuvchiId)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.UpdateWithTransactionNumberAsync(transactionNumber, kassaId, tolovUsuli, sotuvchiId)
            });

        [HttpPut("Nasiyaga/{transactionNumber}")]
        public async Task<IActionResult> UpdateAsync([FromRoute(Name = "transactionNumber")] string transactionNumber, long partnerId, long kassaId, long tolovUsuli, long sotuvchiId)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.NasiyaSavdoAsync(transactionNumber, partnerId, kassaId, tolovUsuli, sotuvchiId)
            });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.RetrieveByIdAsync(id)
            });

        [HttpGet("barcode-orqali-olish/{barCode}")]
        public async Task<IActionResult> GetByBarCodeAsync([FromRoute(Name = "barCode")] string barCode)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.GetByBarCodeAsync(barCode)
            });

        [HttpGet("status-orqali-qidiruv/{status}")]
        public async Task<IActionResult> GetBystatusAsync([FromRoute(Name = "status")] string status)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.SvetUchgandaAsync(status)
            });


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.ReamoveAsync(id)
            });

        [HttpPost("planshetdan-yukni-qaytarish")]
        public async Task<IActionResult> YukniQaytarish(long id, string barCode, decimal quantity)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _korzinkaService.CanceledOrderByKorzinkaAsync(id, barCode, quantity)
            });

    }
}
