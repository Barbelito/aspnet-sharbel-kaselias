using Application.Common.Results;
using Domain.Aggregates.Members;

namespace Application.Members.Abstractions;

public interface IGetMemberProfileService
{
    Task<Result<Member>> ExecuteAsync(string userId, CancellationToken ct = default);
}