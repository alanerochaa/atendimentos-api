using System.ComponentModel.DataAnnotations;


namespace Atendimentos.Application.DTOs;


public class MesaUpdateRequest
{
[Range(1, 50)]
public int? Capacidade { get; set; }


[MaxLength(80)]
public string? Localizacao { get; set; }


[MaxLength(256)]
public string? QrCode { get; set; }
}