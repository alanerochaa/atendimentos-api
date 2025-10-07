using Atendimentos.Domain.Enums;


namespace Atendimentos.Application.DTOs;


public record MesaDto(
Guid Id,
int Numero,
MesaStatus Status,
int? Capacidade,
string? Localizacao,
string? QrCode,
DateTime CreatedAt,
DateTime UpdatedAt
);