// WMS.UnitTests/Features/Auth/LoginCommandHandlerTests.cs
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Auth.Commands;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Services;
using WMS.Application.Common.Interfaces;
using WMS.Application.Common.Settings;
using Xunit;

namespace WMS.UnitTests.Features.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IJwtTokenService> _jwtMock = new();
    private readonly Mock<Microsoft.Extensions.Options.IOptions<JwtSettings>> _jwtOptionsMock = new();

    private LoginCommandHandler CreateHandler()
    {
        _jwtOptionsMock.Setup(o => o.Value).Returns(new JwtSettings
        {
            AccessTokenExpiryMinutes = 60,
            RefreshTokenExpiryDays = 7
        });
        return new LoginCommandHandler(_uowMock.Object, _jwtMock.Object, _jwtOptionsMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test@1234", workFactor: 12);
        var user = new UserLogin
        {
            UserId = 1,
            Username = "admin",
            PasswordHash = passwordHash,
            RoleId = 1,
            Role = new Role { RoleName = "Admin" }
        };

        var mockRepo = new Mock<IGenericRepository<UserLogin>>();
        mockRepo.Setup(r => r.Query()).Returns(new[] { user }.AsQueryable());
        _uowMock.Setup(u => u.UserLogins).Returns(mockRepo.Object);
        _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _jwtMock.Setup(j => j.GenerateAccessToken(user)).Returns("access_token");
        _jwtMock.Setup(j => j.GenerateRefreshToken()).Returns("refresh_token");

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new LoginCommand("admin", "Test@1234"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access_token");
        result.Role.Should().Be("Admin");
    }

    [Fact]
    public async Task Handle_InvalidPassword_ThrowsUnauthorizedException()
    {
        // Arrange
        var user = new UserLogin
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct", workFactor: 12),
            Role = new Role { RoleName = "Admin" }
        };

        var mockRepo = new Mock<IGenericRepository<UserLogin>>();
        mockRepo.Setup(r => r.Query()).Returns(new[] { user }.AsQueryable());
        _uowMock.Setup(u => u.UserLogins).Returns(mockRepo.Object);

        var handler = CreateHandler();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(
            () => handler.Handle(new LoginCommand("admin", "wrong"), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NonExistentUser_ThrowsUnauthorizedException()
    {
        // Arrange
        var mockRepo = new Mock<IGenericRepository<UserLogin>>();
        mockRepo.Setup(r => r.Query()).Returns(Enumerable.Empty<UserLogin>().AsQueryable());
        _uowMock.Setup(u => u.UserLogins).Returns(mockRepo.Object);

        var handler = CreateHandler();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(
            () => handler.Handle(new LoginCommand("nobody", "pass"), CancellationToken.None));
    }
}