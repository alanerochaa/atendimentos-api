using System;
using Atendimentos.Domain.Enums;

namespace Atendimentos.Domain.Entities
{
    public class Comanda
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid MesaId { get; private set; }
        public Guid GarcomId { get; private set; }
        public Guid? ClienteId { get; private set; }

        public ComandaStatus Status { get; private set; } = ComandaStatus.Aberta;
        public DateTime DataHoraAbertura { get; private set; } = DateTime.UtcNow;
        public DateTime? DataHoraFechamento { get; private set; }
        public decimal ValorTotal { get; private set; }

        public Comanda(Guid mesaId, Guid garcomId)
        {
            MesaId = mesaId;
            GarcomId = garcomId;
        }

        public void Fechar(decimal valorTotal)
        {
            ValorTotal = valorTotal;
            Status = ComandaStatus.Fechada;
            DataHoraFechamento = DateTime.UtcNow;
        }
    }
}
