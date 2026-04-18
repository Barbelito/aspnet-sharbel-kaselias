using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.CustomerService;

public class ContactForm
{
    [Required(ErrorMessage = "First name is required")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name *", Prompt = "Enter First Name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name *", Prompt = "Enter Last Name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email address must be valid")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address *", Prompt = "Enter Email Address")]
    public string Email { get; set; } = null!;

    [Phone(ErrorMessage = "Phone number must be valid")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number", Prompt = "Enter Phone Number")]
    public string? Phone {  get; set; }

    [Required(ErrorMessage = "Message is required")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Message *", Prompt = "Message...")]
    public string Message { get; set; } = null!;
}
