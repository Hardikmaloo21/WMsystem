// WMS.Application/Features/Auth/Commands/LogoutCommand.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Auth.Commands;

public record LogoutCommand(string Username) : IRequest<Unit>;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public LogoutCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken ct)
    {
        var user = await _uow.UserLogins.Query()
            .FirstOrDefaultAsync(u => u.Username == request.Username, ct)
            ?? throw new NotFoundException("User not found.");

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await _uow.UserLogins.UpdateAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}