using Application.Common.Results;
using Application.Members.Abstractions;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;

namespace Application.Members.Services;

public class GetMemberProfileService(IMemberRepository memberRepository) : IGetMemberProfileService
{
    public async Task<Result<Member>> ExecuteAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("UserId must not be null.");

            var member = await memberRepository.GetMemberByUserIdAsync(userId, ct);
            return member == null
                ? Result<Member>.NotFound($"Member with user id '{userId}' not found.")
                : Result<Member>.Ok(member);
        }
        catch (Exception ex)
        {
            return Result<Member>.Error(ex.Message);
        }
    }
}
