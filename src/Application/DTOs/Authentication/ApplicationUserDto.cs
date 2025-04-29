using System;

namespace Application.DTOs.Authentication;

public class ApplicationUserDto
{
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
}
