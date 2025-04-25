using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Authentication.DTOs;
public class TokenModelDTO
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;

    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}