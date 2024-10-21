﻿using SmartMarket.Domin.Entities.Users;

namespace SmartMarket.Service.DTOs.Users.Payments;

public record WorkersPaymentForUpdateDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public decimal? Oylik { get; set; }
    public decimal? OlganPuli { get; set; }
    public decimal? QolganPuli { get; set; }
}
