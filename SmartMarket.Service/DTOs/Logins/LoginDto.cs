using System.ComponentModel.DataAnnotations;

namespace SmartMarket.Service.DTOs.Logins;

public class LoginDto
{
    [Required(ErrorMessage = "Telefon raqamni kiriting")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Parolni kiriting")]
    public string Password { get; set; }
}
