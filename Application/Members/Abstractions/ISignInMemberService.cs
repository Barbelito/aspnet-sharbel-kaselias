using Application.Common.Results;
using Application.Members.Inputs;

namespace Application.Members.Abstractions;

public interface ISignInMemberService
{
    Task<Result> ExecuteAsync(SignInInput input, CancellationToken ct = default);
}