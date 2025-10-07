using System.ComponentModel.DataAnnotations;


namespace Atendimentos.Application.DTOs;


public class MesaCreateRequest
{
[Range(1, int.MaxValue)]
public int Numero { get; set; }


[Range(1, 50)]
public int? Capacidade { get; set; }


[MaxLength(80)]
public string? Localizacao { get; set; }


[MaxLength(256)]
public string? QrCode { get; set; }
}