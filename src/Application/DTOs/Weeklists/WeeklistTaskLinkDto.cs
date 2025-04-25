using System;

namespace Application.DTOs.Weeklists;

public class WeeklistTaskLinkDto
{
    public int WeeklistTaskId { get; set; }
    public WeeklistTaskDto? WeeklistTask { get; set; }

    public int WeeklistTaskStatusId { get; set; }
    public WeeklistTaskStatusDto? Status { get; set; }
}
