// WMS.Application/Features/Auth/Commands/LoginCommand.cs
using MediatR;

namespace WMS.Application.Features.Auth.Commands;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    string Username,
    string Role,
    int? EmployeeId
);