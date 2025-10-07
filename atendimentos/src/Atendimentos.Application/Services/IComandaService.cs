using Atendimentos.Domain.Entities;

namespace Atendimentos.Application.Services
{
    public interface IComandaService
    {
        Task<IEnumerable<Comanda>> ObterTodasAsync();
        Task<Comanda?> ObterPorIdAsync(Guid id);
        Task<Comanda> CriarAsync(Guid mesaId, Guid garcomId);
        Task<Comanda?> FecharAsync(Guid id, decimal valorTotal);
        Task<bool> DeletarAsync(Guid id);
    }
}
