using Application.Members.Abstractions;
using Application.Members.Inputs;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Account;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("account")]
public class AccountController(UserManager<ApplicationUser> userManager, IGetMemberProfileService getMemberProfileService, IUpdateMemberProfileService updateMemberProfileService) : Controller
{
    [HttpGet("my")]
    public async Task<IActionResult> My(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var memberProfile = await getMemberProfileService.ExecuteAsync(user.Id, ct);
        if (memberProfile == null)
        {
            return NotFound();
        }

        var viewModel = new MyAccountViewModel
        {
            Email = user.Email ?? string.Empty,
            AboutMeForm = new MyProfileForm
            {
                FirstName = memberProfile.Value?.FirstName ?? string.Empty,
                LastName = memberProfile.Value?.LastName ?? string.Empty,
                PhoneNumber = memberProfile.Value?.PhoneNumber ?? string.Empty,
                ProfileImageUri = memberProfile.Value?.ProfileImageUri ?? string.Empty,
            }
        };
        return View(viewModel);
    }

    [HttpPost("my")]
    public async Task<IActionResult> My(MyAccountViewModel myAccountViewModel, CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            return View(myAccountViewModel);
        }

        myAccountViewModel.Email = user.Email ?? string.Empty;

        var input = new UpdateMemberProfileInput(
            user.Id,
            myAccountViewModel.AboutMeForm.FirstName,
            myAccountViewModel.AboutMeForm.LastName,
            myAccountViewModel.AboutMeForm.PhoneNumber,
            myAccountViewModel.AboutMeForm.ProfileImageUri
        );

        var result = await updateMemberProfileService.ExecuteAsync(input, ct);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Unexpected error occured.");
            return View(myAccountViewModel);
        }

        TempData["SuccessMessage"] = "Your profile has been updated.";
        return RedirectToAction(nameof(My));
    }

}
