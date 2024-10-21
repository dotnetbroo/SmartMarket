using SmartMarket.Domin.Enums;

namespace SmartMarket.Service.DTOs.Users;

public record UserForCreationDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public UserRole Role { get; set; }
}
