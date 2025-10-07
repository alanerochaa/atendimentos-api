using Atendimentos.Domain.Enums;


namespace Atendimentos.Domain.Entities;


public class Mesa
{
public Guid Id { get; private set; } = Guid.NewGuid();
public int Numero { get; private set; }
public MesaStatus Status { get; private set; }
public int? Capacidade { get; private set; }
public string? Localizacao { get; private set; }
public string? QrCode { get; private set; }
public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
public byte[] RowVersion { get; private set; } = Array.Empty<byte>();


private Mesa() { }


public Mesa(int numero, int? capacidade = null, string? localizacao = null, string? qrCode = null)
{
if (numero <= 0) throw new ArgumentException("Numero da mesa deve ser > 0.");
Numero = numero;
Status = MesaStatus.Livre;
Capacidade = capacidade;
Localizacao = localizacao;
QrCode = qrCode;
}


public void Atualizar(int? capacidade, string? localizacao, string? qrCode)
{
Capacidade = capacidade;
Localizacao = localizacao;
QrCode = qrCode;
Touch();
}


public void DefinirStatus(MesaStatus novoStatus)
{
Status = novoStatus;
Touch();
}


private void Touch() => UpdatedAt = DateTime.UtcNow;
}