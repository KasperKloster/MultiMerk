using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Authentication.DTOs;

public class SignupModelDTO
{
    [Required]
    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Password { get; set; } = string.Empty;

}