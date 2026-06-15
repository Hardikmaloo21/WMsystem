// WMS.Application/Features/Auth/Commands/LoginCommandHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Domain.Interfaces;
using WMS.Application.Common.Interfaces;
using WMS.Application.Common.Settings;

namespace WMS.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public LoginCommandHandler(IUnitOfWork uow, IJwtTokenService jwtService,
        Microsoft.Extensions.Options.IOptions<JwtSettings> jwtSettings)
    {
        _uow = uow;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
{
    var user = _uow.UserLogins.Query()
        .Include(u => u.Role)
        .FirstOrDefault(u => u.Username == request.Username)
        ?? throw new UnauthorizedException("Invalid username or password.");

    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        throw new UnauthorizedException("Invalid username or password.");

    var accessToken = _jwtService.GenerateAccessToken(user);
    var refreshToken = _jwtService.GenerateRefreshToken();
    var expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiry = expiry;
    user.LastLogin = DateTime.UtcNow;

    await _uow.UserLogins.UpdateAsync(user, ct);
    await _uow.SaveChangesAsync(ct);

    return new LoginResponse(
        AccessToken: accessToken,
        RefreshToken: refreshToken,
        ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
        Username: user.Username,
        Role: user.Role!.RoleName,
        EmployeeId: user.EmployeeId
    );
}
}