// WMS.Application/Common/Exceptions/NotFoundException.cs
namespace WMS.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string name, object key)
        : base($"{name} with id '{key}' was not found.") { }
}

// WMS.Application/Common/Exceptions/UnauthorizedException.cs


// WMS.Application/Common/Exceptions/ValidationException.cs

// WMS.Application/Common/Exceptions/ConflictException.cs
