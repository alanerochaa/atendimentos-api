using Atendimentos.Application.DTOs;
using Atendimentos.Application.Services;
using Atendimentos.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Atendimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesasController : ControllerBase
    {
        private readonly IMesaService _mesaService;

        public MesasController(IMesaService mesaService)
        {
            _mesaService = mesaService;
        }

        // GET: Listar todas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mesas = await _mesaService.ObterTodasAsync();
            return Ok(mesas);
        }

        // GET: Buscar com filtro, paginação e ordenação
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? order = "asc")
        {
            var todas = await _mesaService.ObterTodasAsync();
            var query = todas.AsQueryable();

            // Filtro
            if (!string.IsNullOrEmpty(status))
                query = query.Where(m => m.Status.ToString().Contains(status, StringComparison.OrdinalIgnoreCase));

            // Ordenação dinâmica
            query = query.OrderByDynamic(sortBy, order);

            // Paginação
            var total = query.Count();
            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                data = result,
                pagination = new
                {
                    totalItems = total,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize)
                }
            });
        }

        // GET: Buscar por ID com HATEOAS
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var mesa = await _mesaService.ObterPorIdAsync(id);
            if (mesa == null) return NotFound();

            var resource = HateoasHelper.BuildResource(this, "Mesas", mesa, id);
            return Ok(resource);
        }

        // POST: Criar mesa
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MesaCreateDto dto)
        {
            var mesa = await _mesaService.CriarMesaAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = mesa.Id }, mesa);
        }

        // PUT: Atualizar mesa
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MesaUpdateDto dto)
        {
            var resultado = await _mesaService.AtualizarMesaAsync(id, dto);
            return resultado == null ? NotFound() : Ok(resultado);
        }

        // PUT: Atualizar status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] MesaStatusUpdateDto dto)
        {
            var resultado = await _mesaService.AtualizarStatusAsync(id, dto.Status);
            return resultado == null ? NotFound() : Ok(resultado);
        }

        // DELETE: Remover mesa
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sucesso = await _mesaService.DeletarMesaAsync(id);
            return !sucesso ? NotFound() : NoContent();
        }
    }
}
