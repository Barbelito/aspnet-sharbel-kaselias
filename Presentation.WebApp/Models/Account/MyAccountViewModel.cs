using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Account;

public class MyAccountViewModel
{
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    public MyProfileForm AboutMeForm { get; set; } = null!;

    public string? ProfileImageUri { get; set; }
}
