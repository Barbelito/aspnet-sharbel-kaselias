using Application.Abstractions.Identity;
using Application.Common.Results;
using Application.Members.Abstractions;
using Application.Members.Inputs;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;

namespace Application.Members.Services;

public class RegisterMemberService(IIdentityService identityService, IMemberRepository memberRepository) : IRegisterMemberService
{
    public async Task<Result<string?>> ExecuteAsync(RegisterMemberInput input, CancellationToken ct = default)
    {
        try
        {
            if (input == null) throw new ArgumentNullException("Input must not be null.");

            var createUserResult = await identityService.CreateUserAsync(input.Email, input.Password, ct);

            if (!createUserResult.Success || string.IsNullOrWhiteSpace(createUserResult.Value))
            {
                return Result<string?>.Error(createUserResult?.ErrorMessage ?? "Unable to create user account");
            }

            var member = Member.Create(createUserResult.Value);

            await memberRepository.AddAsync(member, ct);

            return Result<string?>.Ok(member.UserId);
        }
        catch (Exception ex)
        {
            return Result<string?>.Error($"An error occurred while registering the member account: {ex.Message}");
        }
    }
}
