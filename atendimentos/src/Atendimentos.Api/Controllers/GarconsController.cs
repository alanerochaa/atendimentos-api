using Microsoft.AspNetCore.Mvc;
using Atendimentos.Application.Services;
using Atendimentos.Application.DTOs;
using Atendimentos.Api.Helpers;

namespace Atendimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarconsController : ControllerBase
    {
        private readonly IGarcomService _garcomService;

        public GarconsController(IGarcomService garcomService)
        {
            _garcomService = garcomService;
        }

        // GET: Listar todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var garcons = await _garcomService.ObterTodosAsync();
            return Ok(garcons);
        }

        // GET: Buscar com filtro, paginação e ordenação
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? nome,
            [FromQuery] bool? ativo,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? order = "asc")
        {
            var todos = await _garcomService.ObterTodosAsync();
            var query = todos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(g => g.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

            if (ativo.HasValue)
                query = query.Where(g => g.Ativo == ativo.Value);

            query = query.OrderByDynamic(sortBy, order);

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

        // GET: Buscar por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var garcom = await _garcomService.ObterPorIdAsync(id);
            if (garcom == null) return NotFound();

            var resource = HateoasHelper.BuildResource(this, "Garcons", garcom, id);
            return Ok(resource);
        }

        // POST: Criar
        [HttpPost]
        public async Task<IActionResult> Create(GarcomCreateUpdateDto dto)
        {
            var garcom = await _garcomService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = garcom.Id }, garcom);
        }

        // PUT: Atualizar
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, GarcomCreateUpdateDto dto)
        {
            var atualizado = await _garcomService.AtualizarAsync(id, dto);
            return atualizado == null ? NotFound() : Ok(atualizado);
        }

        // DELETE: Remover
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sucesso = await _garcomService.DeletarAsync(id);
            return !sucesso ? NotFound() : NoContent();
        }
    }
}
