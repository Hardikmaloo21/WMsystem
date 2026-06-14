using System.Security.Claims;
using WMS.Domain.Entities;

namespace WMS.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(UserLogin user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}