namespace Domain.Aggregates.Members;

public class Member
{
    public string Id { get; private set; } = null!;
    public string UserId { get; private set; } = null!;
    public string? FirstName { get; private set; } = null!;
    public string? LastName { get; private set; } = null!;
    public string? PhoneNumber { get; private set; }
    public string? ProfileImageUri { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }

    private Member()
    {

    }

    private Member(string id, string userId, DateTimeOffset createdAt)
    {
        Id = id;
        UserId = userId;
        CreatedAt = createdAt;
    }

    public static Member Create(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
        }
        var member = new Member(
            Guid.NewGuid().ToString(),
            userId,
            DateTimeOffset.UtcNow);

        return member;
    }

    public static Member Rehydrate(string id, string userId, string? firstName, string? lastName, string? phoneNumber, string? profileImageUri, DateTimeOffset createdAt, DateTimeOffset? modifiedAt)
    {
        var member = new Member(id, userId, createdAt)
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            ProfileImageUri = profileImageUri,
            ModifiedAt= modifiedAt
        };
        return member;
    }

    public void Update(string firstName, string lastName, string? phoneNumber, string? profileImageUri)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber;
        ProfileImageUri = string.IsNullOrWhiteSpace(profileImageUri) ? null : profileImageUri;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
}
