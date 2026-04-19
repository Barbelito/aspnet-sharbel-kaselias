using Application.Abstractions.Identity;
using Application.Members.Abstractions;
using Application.Members.Inputs;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Authentication;

namespace Presentation.WebApp.Controllers;

[Route("authentication")]
public class AuthenticationController(
    IRegisterMemberService registerMemberService,
    ISignInMemberService signInMemberService,
    IIdentityService identityService,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager
) : Controller
{
    private const string RegistrationEmailSessionKey = "RegistrationEmail";

    [HttpGet("sign-in")]
    public IActionResult SignIn(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new SignInForm());
    }

    [HttpPost("sign-in")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInForm form, string? returnUrl = null, CancellationToken ct = default)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect email address or password");
            return View(form);
        }

        var user = await userManager.FindByEmailAsync(form.Email);
        if (user is null)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect email address or password");
            return View(form);
        }

        var result = await signInManager.PasswordSignInAsync(
            form.Email,
            form.Password,
            form.RememberMe,
            true
        );

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("My", "Account");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "User account has been temporarily locked");
            return View(form);
        }

        ModelState.AddModelError(nameof(form.ErrorMessage), "Incorrect email address or password");
        return View(form);
    }

    [HttpPost("sign-out")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignOut(CancellationToken ct = default)
    {
        await identityService.SignOutAsync(ct);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("sign-up")]
    public IActionResult SignUp()
    {
        return View(new RegisterEmailForm());
    }

    [HttpPost("sign-up")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(RegisterEmailForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        HttpContext.Session.SetString(RegistrationEmailSessionKey, form.Email);

        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet("set-password")]
    public IActionResult SetPassword()
    {
        var email = HttpContext.Session.GetString(RegistrationEmailSessionKey);
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(SignUp));
        }

        return View(new RegisterPasswordForm());
    }


    [HttpPost("set-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(RegisterPasswordForm form, CancellationToken ct = default)
    {

        var email = HttpContext.Session.GetString(RegistrationEmailSessionKey);
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(SignUp));
        }

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var registerMemberInput = new RegisterMemberInput(email, form.Password);

        var registerResult = await registerMemberService.ExecuteAsync(registerMemberInput, ct);
        if (!registerResult.Success)
        {
            ModelState.AddModelError(nameof(form.Password), registerResult.ErrorMessage ?? "An error occurred while registering.");
            return View(form);
        }
        var signInMemberInput = new SignInInput(email, form.Password, false);

        var signInResult = await signInMemberService.ExecuteAsync(signInMemberInput, ct);

        if(!signInResult.Success)
        {
            ModelState.AddModelError(string.Empty, signInResult.ErrorMessage ?? "Account created but an error occurred while signing in.");
            return View(form);
        }
        HttpContext.Session.Remove(RegistrationEmailSessionKey);

        return RedirectToAction("My", "Account");

    }
}
