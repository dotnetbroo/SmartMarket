namespace SmartMarket.Service.DTOs.Partners;

public record PartnerForCreationDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}
