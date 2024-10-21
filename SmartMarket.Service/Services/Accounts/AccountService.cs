using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Helpers;
using SmartMarket.Service.DTOs.Logins;
using SmartMarket.Service.Interfaces.Accounts;
using SmartMarket.Service.Interfaces.Commons;

namespace SmartMarket.Service.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly IAuthService authService;
    private readonly IRepository<User> userRepository;

    public AccountService(IRepository<User> userRepository, IAuthService authService)
    {
        this.authService = authService;
        this.userRepository = userRepository;
    }
    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await this.userRepository.SelectAll()
            .Where(x => x.PhoneNumber == loginDto.PhoneNumber)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (user is null)
            throw new CustomException(404, "Telefor raqam yoki parol xato kiritildi!");

        var hasherResult = PasswordHelper.Verify(loginDto.Password, user.Salt, user.Password);
        if (hasherResult == false)
            throw new CustomException(404, "Telefor raqam yoki parol xato kiritildi!");

        return authService.GenerateToken(user);
    }
}
