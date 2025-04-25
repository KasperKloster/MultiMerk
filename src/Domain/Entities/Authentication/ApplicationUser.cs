using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Authentication;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
