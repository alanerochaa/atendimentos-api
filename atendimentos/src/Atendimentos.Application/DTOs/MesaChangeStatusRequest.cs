using System.ComponentModel.DataAnnotations;
using Atendimentos.Domain.Enums;


namespace Atendimentos.Application.DTOs;


public class MesaChangeStatusRequest
{
[Required]
public MesaStatus Status { get; set; }
}