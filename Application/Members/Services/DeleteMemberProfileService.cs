using Application.Common.Results;
using Application.Members.Abstractions;
using Application.Members.Inputs;
using Domain.Abstractions.Repositories.Members;

namespace Application.Members.Services;

public class DeleteMemberProfileService(IMemberRepository memberRepository)
    : IDeleteMemberProfileService
{
    public async Task<Result> ExecuteAsync(DeleteMemberProfileInput input, CancellationToken ct = default)
    {
        try
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var member = await memberRepository.GetMemberByUserIdAsync(input.UserId, ct);

            if (member == null)
                return Result.NotFound($"Member with user id '{input.UserId}' not found.");

            var deleted = await memberRepository.DeleteAsync(member, ct);

            return !deleted
                ? Result.Error($"Member with user id '{input.UserId}' could not be deleted.")
                : Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
