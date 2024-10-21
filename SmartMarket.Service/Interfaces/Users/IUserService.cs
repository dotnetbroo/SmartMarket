using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Users;

namespace SmartMarket.Service.Interfaces.Users;

public interface IUserService
{
    Task<bool> RemoveAsync(long id);//
    Task<UserForResultDto> RetrieveByIdAsync(long id);//
    Task<UserForResultDto> CreateAsync(UserForCreationDto dto);//
    Task<UserForResultDto> ModifyAsync(UserForUpdateDto dto);//
    Task<bool> ChangePasswordAsync(UserForChangePasswordDto dto);//
    Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params);//
}