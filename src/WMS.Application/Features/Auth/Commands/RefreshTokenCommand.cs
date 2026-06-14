// WMS.Application/Features/Auth/Commands/RefreshTokenCommand.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Domain.Interfaces;
using WMS.Application.Common.Interfaces;
using WMS.Application.Common.Settings;

namespace WMS.Application.Features.Auth.Commands;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<LoginResponse>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(IUnitOfWork uow, IJwtTokenService jwtService,
        Microsoft.Extensions.Options.IOptions<JwtSettings> jwtSettings)
    {
        _uow = uow;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken)
            ?? throw new UnauthorizedException("Invalid access token.");

        var username = principal.Identity?.Name
            ?? throw new UnauthorizedException("Invalid token claims.");

        var user = await _uow.UserLogins.Query()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username, ct)
            ?? throw new UnauthorizedException("User not found.");

        if (user.RefreshToken != request.RefreshToken ||
            user.RefreshTokenExpiry <= DateTime.UtcNow)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
        await _uow.UserLogins.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return new LoginResponse(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
            Username: user.Username,
            Role: user.Role!.RoleName,
            EmployeeId: user.EmployeeId
        );
    }
}