using Application.Common.Results;
using Application.Members.Inputs;
using Domain.Aggregates.Members;

namespace Application.Members.Abstractions;

public interface IUpdateMemberProfileService
{
    Task<Result<Member>> ExecuteAsync(UpdateMemberProfileInput input, CancellationToken ct = default);
}