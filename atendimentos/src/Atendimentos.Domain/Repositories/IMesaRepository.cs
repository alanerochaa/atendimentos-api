using Atendimentos.Domain.Entities;
using Atendimentos.Domain.Enums;


namespace Atendimentos.Domain.Repositories;


public interface IMesaRepository
{
Task<Mesa?> GetByIdAsync(Guid id, CancellationToken ct = default);
Task<Mesa?> GetByNumeroAsync(int numero, CancellationToken ct = default);
Task<List<Mesa>> ListAsync(MesaStatus? status = null, CancellationToken ct = default);
Task AddAsync(Mesa mesa, CancellationToken ct = default);
Task UpdateAsync(Mesa mesa, CancellationToken ct = default);
Task SaveChangesAsync(CancellationToken ct = default);
}