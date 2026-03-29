using Application.Abstractions.Identity;
using Application.Common.Results;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IIdentityService
{
    public async Task<Result<string?>> CreateUserAsync(string email, string password, CancellationToken ct = default)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return Result<string?>.Conflict("A user with this email already exists.");

        var newUser = ApplicationUser.Create(email);

        var result = await userManager.CreateAsync(newUser, password);
        if (!result.Succeeded)
        {
            var error = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result<string?>.Error(error);
        }

        return Result<string?>.Ok(newUser.Id);

    }

    public async Task<Result<bool>> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct = default)
    {
        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
            return Result<bool>.Ok(true);

        if (result.IsLockedOut)
            return Result<bool>.Error("Your account is locked out.");

        if (result.IsNotAllowed)
            return Result<bool>.Error("You are not allowed to sign in.");

        if (result.RequiresTwoFactor)
            return Result<bool>.Error("Two-factor authentication is required.");

        return Result<bool>.Error("Invalid email or password.");
    }

    public Task SignOutAsync(CancellationToken ct = default)
    {
        return signInManager.SignOutAsync();
    }
}
