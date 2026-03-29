using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication;

public class RegisterPasswordForm
{
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "You must confirm your password.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
    public string ConfirmPassword { get; set; } = null!;
}
