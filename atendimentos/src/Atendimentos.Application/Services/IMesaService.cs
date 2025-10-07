using Atendimentos.Application.DTOs;
using Atendimentos.Domain.Enums;


namespace Atendimentos.Application.Services;


public interface IMesaService
{
Task<MesaDto> CreateAsync(MesaCreateRequest input, CancellationToken ct = default);
Task<MesaDto?> GetAsync(Guid id, CancellationToken ct = default);
Task<IReadOnlyList<MesaDto>> ListAsync(MesaStatus? status, CancellationToken ct = default);
Task<bool> UpdateAsync(Guid id, MesaUpdateRequest input, CancellationToken ct = default);
Task<bool> ChangeStatusAsync(Guid id, MesaChangeStatusRequest input, CancellationToken ct = default);
}