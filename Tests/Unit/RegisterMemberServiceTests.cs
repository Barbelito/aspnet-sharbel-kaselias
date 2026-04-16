using Application.Abstractions.Identity;
using Application.Common.Results;
using Application.Members.Inputs;
using Application.Members.Services;
using Domain.Abstractions.Repositories.Members;
using Domain.Aggregates.Members;
using Moq;
using Xunit;

public class RegisterMemberServiceTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnOk_WhenUserIsCreatedSuccessfully()
    {
        // Arrange
        var identityMock = new Mock<IIdentityService>();
        var memberRepoMock = new Mock<IMemberRepository>();

        identityMock
            .Setup(x => x.CreateUserAsync("test@test.com", "Password123", default))
            .ReturnsAsync(Result<string?>.Ok("new-user-id"));

        var service = new RegisterMemberService(identityMock.Object, memberRepoMock.Object);

        var input = new RegisterMemberInput("test@test.com", "Password123");

        // Act
        var result = await service.ExecuteAsync(input);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("new-user-id", result.Value);
        memberRepoMock.Verify(x => x.AddAsync(It.IsAny<Member>(), default), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenIdentityServiceFails()
    {
        // Arrange
        var identityMock = new Mock<IIdentityService>();
        var memberRepoMock = new Mock<IMemberRepository>();

        identityMock
            .Setup(x => x.CreateUserAsync("fail@test.com", "123", default))
            .ReturnsAsync(Result<string?>.Error("Identity error"));

        var service = new RegisterMemberService(identityMock.Object, memberRepoMock.Object);

        var input = new RegisterMemberInput("fail@test.com", "123");

        // Act
        var result = await service.ExecuteAsync(input);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Identity error", result.ErrorMessage);
        memberRepoMock.Verify(x => x.AddAsync(It.IsAny<Member>(), default), Times.Never);
    }
    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenInputIsNull()
    {
        // Arrange
        var identityMock = new Mock<IIdentityService>();
        var memberRepoMock = new Mock<IMemberRepository>();

        var service = new RegisterMemberService(identityMock.Object, memberRepoMock.Object);

        // Act
        var result = await service.ExecuteAsync(null);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Input must not be null", result.ErrorMessage);
        memberRepoMock.Verify(x => x.AddAsync(It.IsAny<Member>(), default), Times.Never);
    }
}


