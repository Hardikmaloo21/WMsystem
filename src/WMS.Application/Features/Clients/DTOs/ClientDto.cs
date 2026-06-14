// WMS.Application/Features/Clients/DTOs/ClientDto.cs
namespace WMS.Application.Features.Clients.DTOs;

public class ClientDto
{
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? ClientAddress { get; set; }
    public decimal? ClientPhoneNumber { get; set; }
    public string? ClientLocation { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedOn { get; set; }
}

public record CreateClientDto(
    string ClientName,
    string? ClientAddress,
    decimal? ClientPhoneNumber,
    string? ClientLocation);

public record UpdateClientDto(
    int ClientId,
    string ClientName,
    string? ClientAddress,
    decimal? ClientPhoneNumber,
    string? ClientLocation,
    bool Status);