using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication;

public class SignInForm
{
    [Required(ErrorMessage = "Email is required.")]
     [Display(Name = "Email Address", Prompt = "Enter Email Address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}