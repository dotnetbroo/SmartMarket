using Microsoft.AspNetCore.Mvc;
using SmartMarket.Api.Models;
using SmartMarket.Service.DTOs.Logins;
using SmartMarket.Service.Interfaces.Accounts;
using SmartMarket.Service.Interfaces.Commons;

namespace SmartMarket.Api.Controllers.Commons
{
    public class AuthController : BaseController
    {
        private readonly IAccountService accountService;

        public AuthController(IAccountService accountService, IAuthService authService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("login")]
        public async ValueTask<IActionResult> login([FromBody] LoginDto loginDto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await accountService.LoginAsync(loginDto)
            });
    }
}
