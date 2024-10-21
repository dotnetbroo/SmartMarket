using SmartMarket.Service.DTOs.Products;
using SmartMarket.Service.DTOs.Tolov;

namespace SmartMarket.Service.DTOs.ContrAgents;

public record ContrAgentForResultDto
{
    public long Id { get; set; }
    public string Firma { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public decimal Dept { get; set; }
    public decimal PayForDept { get; set; }
    public decimal LastPaid { get; set; }
    public long TolovUsuliID { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public IEnumerable<TolovForResultDto> Tolovs { get; set; }
    public IEnumerable<ProductForResultDto> Products { get; set; }
}
