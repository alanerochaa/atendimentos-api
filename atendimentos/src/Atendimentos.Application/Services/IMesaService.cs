using Atendimentos.Application.DTOs;

namespace Atendimentos.Application.Services
{
    public interface IMesaService
    {
        Task<MesaDto> CriarMesaAsync(MesaCreateDto dto);
        Task<IEnumerable<MesaDto>> ObterTodasAsync();
        Task<MesaDto?> ObterPorIdAsync(Guid id); // ✅ novo método
        Task<MesaDto?> AtualizarMesaAsync(Guid id, MesaUpdateDto dto);
        Task<MesaDto?> AtualizarStatusAsync(Guid id, int novoStatus);
        Task<bool> DeletarMesaAsync(Guid id);
    }
}
