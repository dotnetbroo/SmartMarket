﻿namespace SmartMarket.Service.DTOs.ContrAgents;

public record ContrAgentForUpdateDto
{
    public long Id { get; set; }
    public string Firma { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}
