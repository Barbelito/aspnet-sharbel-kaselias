using Infrastructure.Identity;

namespace Infrastructure.Persistence.Entities.Members;

public class MemberEntity
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUri { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
