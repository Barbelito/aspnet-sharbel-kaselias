using Application.Members.Abstractions;
using Application.Members.Inputs;
using Application.Members.Services;
using Domain.Abstractions.Repositories.Members;
using Infrastructure.Extensions.Identity;
using Infrastructure.Persistence.EfCore.Contexts;
using Infrastructure.Persistence.EfCore.Repositories.Members;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class RegisterMemberServiceIntegrationTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateMemberInDatabase_WhenUserIsCreatedSuccessfully()
    {
        // -------------------------
        // Arrange: SQLite in-memory
        // -------------------------
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var services = new ServiceCollection();

        // Required for ASP.NET Identity
        services.AddLogging();

        // EF Core
        services.AddDbContext<DataContext>(options =>
            options.UseSqlite(connection));

        var initProvider = services.BuildServiceProvider();

        using (var scope = initProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            db.Database.EnsureCreated();
        }

        // Application services
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IRegisterMemberService, RegisterMemberService>();

        // Identity (REAL)
        services.AddIdentityServices();

        var provider = services.BuildServiceProvider();

        var service = provider.GetRequiredService<IRegisterMemberService>();

        var input = new RegisterMemberInput(
            "test@test.com",
            "Password123.123");

        // -------------------------
        // Act
        // -------------------------
        var result = await service.ExecuteAsync(input);

        // -------------------------
        // Assert: service result
        // -------------------------
        Assert.True(result.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Value));

        // -------------------------
        // Assert: database state
        // -------------------------
        using (var scope = provider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();

            var savedMember = await db.Members
                .FirstOrDefaultAsync(x => x.UserId == result.Value);

            Assert.NotNull(savedMember);
            Assert.Equal(result.Value, savedMember.UserId);
        }
    }
}