using System;

namespace Application.DTOs.Weeklists;

public class WeeklistTaskStatusDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
}
