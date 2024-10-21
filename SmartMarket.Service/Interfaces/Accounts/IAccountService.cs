using SmartMarket.Service.DTOs.Logins;

namespace SmartMarket.Service.Interfaces.Accounts;

public interface IAccountService
{
    public Task<string> LoginAsync(LoginDto loginDto);
}

