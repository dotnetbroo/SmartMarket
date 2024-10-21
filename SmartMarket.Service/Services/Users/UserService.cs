using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.Commons.Helpers;
using SmartMarket.Service.DTOs.Users;
using SmartMarket.Service.Interfaces.Users;

namespace SmartMarket.Service.Services.Users;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;
    public UserService(IRepository<User> userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserForResultDto> CreateAsync(UserForCreationDto dto)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.PhoneNumber == dto.PhoneNumber)
            .FirstOrDefaultAsync();
        if (user is not null)
            throw new CustomException(403, "Bunday ishchi mavjud.");

        var hasherResult = PasswordHelper.Hash(dto.Password);
        var mapped = _mapper.Map<User>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Salt = hasherResult.Salt;
        mapped.Password = hasherResult.Hash;
        var result = await _userRepository.InsertAsync(mapped);
        return _mapper.Map<UserForResultDto>(result);
    }

    public async Task<UserForResultDto> ModifyAsync(UserForUpdateDto dto)
    {
        var user = await _userRepository.SelectAll()
             .Where(u => u.Id == dto.Id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Ishchi topilmadi.");

        var mapped = _mapper.Map(dto, user);
        mapped.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(mapped);

        return _mapper.Map<UserForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var user = await _userRepository.SelectAll()
              .Where(u => u.Id == id)
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Ishchi topilmadi.");

        await _userRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await _userRepository.SelectAll()
             .Include(p => p.YukYiguvchi)
             .Include(y => y.YukTaxlovchi)
             .Include(t => t.Taxlovchi)
             .Include(ty => ty.Yiguvchi)
             .Include(y => y.YiguvchiCards)
             .Include(uc => uc.CasherCards)
             .Include(yk => yk.YukTaxlovchisi)
             .Include(p => p.Products)
             .Include(c => c.PartnerProducts)
             .AsNoTracking()
             .ToPagedList(@params)
             .ToListAsync();

        return _mapper.Map<IEnumerable<UserForResultDto>>(users);
    }

    public async Task<UserForResultDto> RetrieveByIdAsync(long id)
    {
        var user = await _userRepository.SelectAll()
             .Where(u => u.Id == id)
             .Include(p => p.Payments)
             .Include(p => p.YukYiguvchi)
             .Include(y => y.YukTaxlovchi)
             .Include(t => t.Taxlovchi)
             .Include(ty => ty.Yiguvchi)
             .Include(y => y.YiguvchiCards)
             .Include(uc => uc.CasherCards)
             .Include(yk => yk.YukTaxlovchisi)
             .Include(p => p.Products)
             .Include(c => c.PartnerProducts)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Ishchi topilmadi.");

        return _mapper.Map<UserForResultDto>(user);
    }

    public async Task<bool> ChangePasswordAsync(UserForChangePasswordDto dto)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == dto.Id)
            .FirstOrDefaultAsync();
        if (user is null || !PasswordHelper.Verify(dto.OldPassword, user.Salt, user.Password))
            throw new CustomException(404, "Eski parol xato!");
        else if (dto.NewPassword != dto.ConfirmPassword)
            throw new CustomException(400, "Yangi parol va tasdiqlash paroli bir xil emas!\nXatolikni to'g'rilang!");

        var hash = PasswordHelper.Hash(dto.ConfirmPassword);
        user.Salt = hash.Salt;
        user.Password = hash.Hash;

        await _userRepository.UpdateAsync(user);

        return true;
    }
}
