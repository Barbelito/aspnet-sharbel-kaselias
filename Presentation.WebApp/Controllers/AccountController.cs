using Application.Members.Abstractions;
using Application.Members.Inputs;
using Application.Members.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Account;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("account")]
public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IGetMemberProfileService getMemberProfileService,
    IUpdateMemberProfileService updateMemberProfileService,
    IDeleteMemberProfileService deleteMemberProfileService,
    IWebHostEnvironment env) : Controller
{
    [HttpGet("my")]
    public async Task<IActionResult> My(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var memberProfile = await getMemberProfileService.ExecuteAsync(user.Id, ct);
        if (memberProfile == null)
            return NotFound();

        var viewModel = new MyAccountViewModel
        {
            Email = user.Email ?? string.Empty,
            ProfileImageUri = memberProfile.Value?.ProfileImageUri,
            AboutMeForm = new MyProfileForm
            {
                FirstName = memberProfile.Value?.FirstName ?? string.Empty,
                LastName = memberProfile.Value?.LastName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = memberProfile.Value?.PhoneNumber ?? string.Empty
            }
        };

        return View(viewModel);
    }

    [HttpPost("my")]
    public async Task<IActionResult> My(MyAccountViewModel vm, CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        if (!ModelState.IsValid)
            return View(vm);

        string? imageUrl = vm.ProfileImageUri;

        // Handle file upload
        if (vm.AboutMeForm.ProfileImage is not null)
        {
            var file = vm.AboutMeForm.ProfileImage;
            var uploadsPath = Path.Combine(env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            imageUrl = $"/uploads/{fileName}";
        }

        var input = new UpdateMemberProfileInput(
            user.Id,
            vm.AboutMeForm.FirstName,
            vm.AboutMeForm.LastName,
            vm.AboutMeForm.PhoneNumber,
            imageUrl
        );

        var result = await updateMemberProfileService.ExecuteAsync(input, ct);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Unexpected error occurred.");
            return View(vm);
        }

        TempData["SuccessMessage"] = "Your profile has been updated.";
        return RedirectToAction(nameof(My));
    }
    [HttpGet("signout")]
    public new async Task<IActionResult> SignOut()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    [HttpGet("delete")]
    public IActionResult DeleteAccount()
    {
        return View();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteAccountConfirmed(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return Challenge();

        var deleteProfileInput = new DeleteMemberProfileInput(user.Id);
        await deleteMemberProfileService.ExecuteAsync(deleteProfileInput, ct);


        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Failed to delete account.");
            return View("DeleteAccount");
        }
        await HttpContext.SignOutAsync();

        TempData["SuccessMessage"] = "Your account has been deleted.";
        return RedirectToAction("Index", "Home");
    }

}
