using Atendimentos.Application.DTOs;
using Atendimentos.Domain.Entities;
using Atendimentos.Domain.Repositories;

namespace Atendimentos.Application.Services
{
    public class MesaService : IMesaService
    {
        private readonly IMesaRepository _mesaRepository;

        public MesaService(IMesaRepository mesaRepository)
        {
            _mesaRepository = mesaRepository;
        }

        public async Task<MesaDto> CriarMesaAsync(MesaCreateDto dto)
        {
            var mesa = new Mesa(dto.Numero, dto.Capacidade, dto.Localizacao, dto.QrCode);
            await _mesaRepository.CriarAsync(mesa);

            return MapToDto(mesa);
        }

        public async Task<IEnumerable<MesaDto>> ObterTodasAsync()
        {
            var mesas = await _mesaRepository.ObterTodasAsync();
            return mesas.Select(MapToDto);
        }

        //  buscar mesa por ID
        public async Task<MesaDto?> ObterPorIdAsync(Guid id)
        {
            var mesa = await _mesaRepository.ObterPorIdAsync(id);
            if (mesa == null)
                return null;

            return MapToDto(mesa);
        }

        public async Task<MesaDto?> AtualizarMesaAsync(Guid id, MesaUpdateDto dto)
        {
            var mesa = await _mesaRepository.ObterPorIdAsync(id);
            if (mesa == null)
                return null;

            mesa.AtualizarDados(dto.Capacidade, dto.Localizacao, dto.QrCode);
            await _mesaRepository.AtualizarAsync(mesa);

            return MapToDto(mesa);
        }

        public async Task<MesaDto?> AtualizarStatusAsync(Guid id, int novoStatus)
        {
            var mesa = await _mesaRepository.ObterPorIdAsync(id);
            if (mesa == null)
                return null;

            mesa.AlterarStatus((Domain.Enums.MesaStatus)novoStatus);
            await _mesaRepository.AtualizarAsync(mesa);

            return MapToDto(mesa);
        }

        public async Task<bool> DeletarMesaAsync(Guid id)
        {
            var mesa = await _mesaRepository.ObterPorIdAsync(id);
            if (mesa == null)
                return false;

            await _mesaRepository.DeletarAsync(id);
            return true;
        }

        // Mapeamento para DTO
        private static MesaDto MapToDto(Mesa mesa) => new MesaDto
        {
            Id = mesa.Id,
            Numero = mesa.Numero,
            Status = (int)mesa.Status,
            Capacidade = mesa.Capacidade,
            Localizacao = mesa.Localizacao,
            QrCode = mesa.QrCode,
            CreatedAt = mesa.CreatedAt,
            UpdatedAt = mesa.UpdatedAt
        };
    }
}
