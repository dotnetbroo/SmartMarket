namespace SmartMarket.Service.DTOs.Categories;

public record CategoryForUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}
