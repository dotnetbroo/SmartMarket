using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Categories;
using SmartMarket.Service.Interfaces.Categories;

namespace SmartMarket.Api.Controllers.Categories
{
    [Authorize]
    public class CategoriesController : BaseController
    {
        ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CategoryForCreationDto dto)
           => Ok(new Response
           {
               Code = 200,
               Message = "Success",
               Data = await _categoryService.CreateAsync(dto)
           });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _categoryService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _categoryService.RetrieveByIdAsync(id)
            });

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] CategoryForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _categoryService.ModifyAsync(dto)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _categoryService.ReamoveAsync(id)
            });
    }
}
