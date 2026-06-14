// WMS.Application/Features/Auth/Commands/ChangePasswordCommand.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Auth.Commands;

public record ChangePasswordCommand(
    string Username,
    string CurrentPassword,
    string NewPassword
) : IRequest<Unit>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public ChangePasswordCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var user = await _uow.UserLogins.Query()
            .FirstOrDefaultAsync(u => u.Username == request.Username, ct)
            ?? throw new NotFoundException("User not found.");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedException("Current password is incorrect.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: 12);
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await _uow.UserLogins.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}