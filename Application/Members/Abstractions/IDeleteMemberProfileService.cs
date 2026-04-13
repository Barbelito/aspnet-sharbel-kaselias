using Application.Common.Results;
using Application.Members.Inputs;

namespace Application.Members.Abstractions;

public interface IDeleteMemberProfileService
{
    Task<Result> ExecuteAsync(DeleteMemberProfileInput input, CancellationToken ct = default);
}
