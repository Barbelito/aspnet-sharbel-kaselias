using Application.Abstractions.Identity;
using Application.Members.Abstractions;
using Application.Members.Inputs;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Authentication;

namespace Presentation.WebApp.Controllers;

[Route("authentication")]
public class AuthenticationController(IRegisterMemberService registerMemberService, ISignInMemberService signInMemberService, IIdentityService identityService) : Controller
{
    private const string RegistrationEmailSessionKey = "RegistrationEmail";

    [HttpGet("sign-in")]
    public IActionResult SignIn()
    {
        return View(new SignInForm());
    }

    [HttpPost("sign-in")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var input = new SignInInput(form.Email, form.Password, form.RememberMe);

        var result = await signInMemberService.ExecuteAsync(input, ct);

        if(!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "An error occurred while signing in.");
            return View(form);
        }
        return RedirectToAction("My", "Account");
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
        if(!registerResult.Success)
        {
            ModelState.AddModelError(string.Empty, registerResult.ErrorMessage ?? "An error occurred while registering.");
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
