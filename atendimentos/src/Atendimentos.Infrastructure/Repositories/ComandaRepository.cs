using Atendimentos.Domain.Entities;
using Atendimentos.Domain.Repositories;
using Atendimentos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Atendimentos.Infrastructure.Repositories
{
    public class ComandaRepository : IComandaRepository
    {
        private readonly AtendimentosDbContext _context;

        public ComandaRepository(AtendimentosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comanda>> ObterTodasAsync()
        {
            return await _context.Comandas.ToListAsync();
        }

        public async Task<Comanda?> ObterPorIdAsync(Guid id)
        {
            return await _context.Comandas.FindAsync(id);
        }

        public async Task CriarAsync(Comanda comanda)
        {
            _context.Comandas.Add(comanda);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Comanda comanda)
        {
            _context.Comandas.Update(comanda);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var comanda = await _context.Comandas.FindAsync(id);
            if (comanda != null)
            {
                _context.Comandas.Remove(comanda);
                await _context.SaveChangesAsync();
            }
        }
    }
}
