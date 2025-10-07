using Atendimentos.Domain.Entities;
using Atendimentos.Domain.Enums;
using Atendimentos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Atendimentos.Infrastructure.Repositories;


public class MesaRepository : IMesaRepository
{
private readonly AtendimentosDbContext _ctx;


public MesaRepository(AtendimentosDbContext ctx) => _ctx = ctx;


public Task<Mesa?> GetByIdAsync(Guid id, CancellationToken ct = default)
=> _ctx.Mesas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);


public Task<Mesa?> GetByNumeroAsync(int numero, CancellationToken ct = default)
=> _ctx.Mesas.AsNoTracking().FirstOrDefaultAsync(x => x.Numero == numero, ct);


public async Task<List<Mesa>> ListAsync(MesaStatus? status = null, CancellationToken ct = default)
{
var q = _ctx.Mesas.AsNoTracking().AsQueryable();
if (status.HasValue) q = q.Where(x => x.Status == status);
return await q.OrderBy(x => x.Numero).ToListAsync(ct);
}


public async Task AddAsync(Mesa mesa, CancellationToken ct = default)
=> await _ctx.Mesas.AddAsync(mesa, ct);


public Task UpdateAsync(Mesa mesa, CancellationToken ct = default)
{
_ctx.Mesas.Update(mesa);
return Task.CompletedTask;
}


public Task SaveChangesAsync(CancellationToken ct = default)
=> _ctx.SaveChangesAsync(ct);
}