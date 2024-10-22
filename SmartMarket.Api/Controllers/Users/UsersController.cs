using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Controllers.Commons;
using SmartMarket.Api.Models;
using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Users;
using SmartMarket.Service.Interfaces.Users;

namespace SmartMarket.Api.Controllers.Users
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
         
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromForm] UserForCreationDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _userService.CreateAsync(dto)
            });

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(await _userService.RetrieveAllAsync(@params));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _userService.RetrieveByIdAsync(id)
            });

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _userService.RemoveAsync(id)
            });

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm]UserForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _userService.ModifyAsync(dto)
            });

        [HttpPut("change-password")]
        public async Task<ActionResult<UserForResultDto>> ChangePasswordAsync([FromBody] UserForChangePasswordDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await _userService.ChangePasswordAsync(dto)
            });
    }
}
