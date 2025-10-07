using Atendimentos.Application.DTOs;
using Atendimentos.Domain.Entities;
using Atendimentos.Domain.Enums;
using Atendimentos.Domain.Repositories;


namespace Atendimentos.Application.Services;


public class MesaService : IMesaService
{
private readonly IMesaRepository _repo;


public MesaService(IMesaRepository repo)
{
_repo = repo;
}


public async Task<MesaDto> CreateAsync(MesaCreateRequest input, CancellationToken ct = default)
{
var jaExiste = await _repo.GetByNumeroAsync(input.Numero, ct);
if (jaExiste is not null) throw new InvalidOperationException($"Já existe mesa com número {input.Numero}.");


var mesa = new Mesa(input.Numero, input.Capacidade, input.Localizacao, input.QrCode);
await _repo.AddAsync(mesa, ct);
await _repo.SaveChangesAsync(ct);
return ToDto(mesa);
}


public async Task<MesaDto?> GetAsync(Guid id, CancellationToken ct = default)
{
var mesa = await _repo.GetByIdAsync(id, ct);
return mesa is null ? null : ToDto(mesa);
}


public async Task<IReadOnlyList<MesaDto>> ListAsync(MesaStatus? status, CancellationToken ct = default)
{
var list = await _repo.ListAsync(status, ct);
return list.Select(ToDto).ToList();
}


public async Task<bool> UpdateAsync(Guid id, MesaUpdateRequest input, CancellationToken ct = default)
{
var mesa = await _repo.GetByIdAsync(id, ct);
if (mesa is null) return false;
mesa.Atualizar(input.Capacidade, input.Localizacao, input.QrCode);
await _repo.UpdateAsync(mesa, ct);
await _repo.SaveChangesAsync(ct);
return true;
}


public async Task<bool> ChangeStatusAsync(Guid id, MesaChangeStatusRequest input, CancellationToken ct = default)
{
var mesa = await _repo.GetByIdAsync(id, ct);
if (mesa is null) return false;
mesa.DefinirStatus(input.Status);
await _repo.UpdateAsync(mesa, ct);
await _repo.SaveChangesAsync(ct);
return true;
}


private static MesaDto ToDto(Mesa m) => new(
m.Id, m.Numero, m.Status, m.Capacidade, m.Localizacao, m.QrCode, m.CreatedAt, m.UpdatedAt
);
}