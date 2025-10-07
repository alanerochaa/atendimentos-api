using Atendimentos.Application.DTOs;
using Atendimentos.Application.Services;
using Atendimentos.Domain.Enums;
using Microsoft.AspNetCore.Mvc;


namespace Atendimentos.Api.Controllers;


[ApiController]
[Route("mesas")]
public class MesasController : ControllerBase
{
private readonly IMesaService _service;


public MesasController(IMesaService service)
{
_service = service;
}


[HttpGet]
public async Task<ActionResult<IEnumerable<MesaDto>>> List([FromQuery] MesaStatus? status, CancellationToken ct)
=> Ok(await _service.ListAsync(status, ct));


[HttpGet("{id:guid}")]
public async Task<ActionResult<MesaDto>> Get(Guid id, CancellationToken ct)
{
var mesa = await _service.GetAsync(id, ct);
return mesa is null ? NotFound() : Ok(mesa);
}


[HttpPost]
public async Task<ActionResult<MesaDto>> Create([FromBody] MesaCreateRequest body, CancellationToken ct)
{
if (!ModelState.IsValid) return ValidationProblem(ModelState);


try
{
var created = await _service.CreateAsync(body, ct);
return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
}
catch (InvalidOperationException ex)
{
return Conflict(new { message = ex.Message });
}
}


[HttpPut("{id:guid}")]
public async Task<ActionResult> Update(Guid id, [FromBody] MesaUpdateRequest body, CancellationToken ct)
{
if (!ModelState.IsValid) return ValidationProblem(ModelState);
var ok = await _service.UpdateAsync(id, body, ct);
return ok ? NoContent() : NotFound();
}


[HttpPut("{id:guid}/status")]
public async Task<ActionResult> ChangeStatus(Guid id, [FromBody] MesaChangeStatusRequest body, CancellationToken ct)
{
if (!ModelState.IsValid) return ValidationProblem(ModelState);
var ok = await _service.ChangeStatusAsync(id, body, ct);
return ok ? NoContent() : NotFound();
}
}