using SmartMarket.Domin.Entities.Users;

namespace SmartMarket.Service.Interfaces.Commons;

public interface IAuthService
{
    public string GenerateToken(User users);
}
