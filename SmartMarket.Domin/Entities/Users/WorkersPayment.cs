using SmartMarket.Domin.Commons;

namespace SmartMarket.Domin.Entities.Users;

public class WorkersPayment : Auditable
{
    public long UserId {  get; set; }
    public User User { get; set; }
    public decimal? Oylik { get; set; }
    public decimal? OlganPuli { get; set; }
    public decimal? QolganPuli { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
