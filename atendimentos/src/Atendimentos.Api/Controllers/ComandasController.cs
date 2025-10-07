using Atendimentos.Application.Services;
using Atendimentos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Atendimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComandasController : ControllerBase
    {
        private readonly IComandaService _service;

        public ComandasController(IComandaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comandas = await _service.ObterTodasAsync();
            return Ok(comandas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comanda = await _service.ObterPorIdAsync(id);
            return comanda == null ? NotFound() : Ok(comanda);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] Guid mesaId, [FromQuery] Guid garcomId)
        {
            var comanda = await _service.CriarAsync(mesaId, garcomId);
            return CreatedAtAction(nameof(GetById), new { id = comanda.Id }, comanda);
        }

        [HttpPut("{id}/fechar")]
        public async Task<IActionResult> Fechar(Guid id, [FromQuery] decimal valorTotal)
        {
            var comanda = await _service.FecharAsync(id, valorTotal);
            return comanda == null ? NotFound() : Ok(comanda);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sucesso = await _service.DeletarAsync(id);
            return sucesso ? NoContent() : NotFound();
        }
    }
}
