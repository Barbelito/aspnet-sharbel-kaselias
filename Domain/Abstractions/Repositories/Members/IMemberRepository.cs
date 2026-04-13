using Domain.Aggregates.Members;

namespace Domain.Abstractions.Repositories.Members;

public interface IMemberRepository : IRepositoryBase<Member, string>
{
    Task<Member?> GetMemberByUserIdAsync(string userId, CancellationToken ct = default);

    string GetUserId(Member model);

    Task<bool> DeleteAsync(Member member, CancellationToken ct = default);

}
