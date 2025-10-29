using Microsoft.AspNetCore.Mvc;
using Atendimentos.Application.Services;
using Atendimentos.Api.Helpers;

namespace Atendimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            _service = service;
        }

        // POST: Criar cliente
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ClienteCreateDto dto)
        {
            var cliente = await _service.CriarAsync(dto.Nome, dto.CPF, dto.Telefone);
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
        }

        // GET: Listar todos
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var clientes = await _service.ObterTodosAsync();
            return Ok(clientes);
        }

        // GET: Buscar com filtro, paginação e ordenação
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? nome,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? order = "asc")
        {
            var todos = await _service.ObterTodosAsync();
            var query = todos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

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
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var cliente = await _service.ObterPorIdAsync(id);
            if (cliente == null) return NotFound();

            var resource = HateoasHelper.BuildResource(this, "Clientes", cliente, id);
            return Ok(resource);
        }

        // DELETE: Remover cliente
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _service.DeletarAsync(id);
            return NoContent();
        }
    }

    public class ClienteCreateDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}
