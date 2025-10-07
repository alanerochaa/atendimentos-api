using Atendimentos.Domain.Entities;

namespace Atendimentos.Domain.Repositories
{
    public interface IComandaRepository
    {
        Task<IEnumerable<Comanda>> ObterTodasAsync();
        Task<Comanda?> ObterPorIdAsync(Guid id);
        Task CriarAsync(Comanda comanda);
        Task AtualizarAsync(Comanda comanda);
        Task DeletarAsync(Guid id);
    }
}
