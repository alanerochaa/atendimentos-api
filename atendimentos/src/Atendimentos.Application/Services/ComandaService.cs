using Atendimentos.Domain.Entities;
using Atendimentos.Domain.Repositories;

namespace Atendimentos.Application.Services
{
    public class ComandaService : IComandaService
    {
        private readonly IComandaRepository _repository;

        public ComandaService(IComandaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Comanda>> ObterTodasAsync()
        {
            return await _repository.ObterTodasAsync();
        }

        public async Task<Comanda?> ObterPorIdAsync(Guid id)
        {
            return await _repository.ObterPorIdAsync(id);
        }

        public async Task<Comanda> CriarAsync(Guid mesaId, Guid garcomId)
        {
            var comanda = new Comanda(mesaId, garcomId);
            await _repository.CriarAsync(comanda);
            return comanda;
        }

        public async Task<Comanda?> FecharAsync(Guid id, decimal valorTotal)
        {
            var comanda = await _repository.ObterPorIdAsync(id);
            if (comanda == null) return null;

            comanda.Fechar(valorTotal);
            await _repository.AtualizarAsync(comanda);
            return comanda;
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var comanda = await _repository.ObterPorIdAsync(id);
            if (comanda == null) return false;

            await _repository.DeletarAsync(id);
            return true;
        }
    }
}
