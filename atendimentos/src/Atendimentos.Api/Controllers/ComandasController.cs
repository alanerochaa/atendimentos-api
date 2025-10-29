using Microsoft.AspNetCore.Mvc;
using Atendimentos.Application.Services;
using Atendimentos.Api.Helpers;

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

        // GET: Listar todas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comandas = await _service.ObterTodasAsync();
            return Ok(comandas);
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
            var todas = await _service.ObterTodasAsync();
            var query = todas.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status.ToString().Contains(status, StringComparison.OrdinalIgnoreCase));

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

        // GET: Buscar por ID com HATEOAS
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comanda = await _service.ObterPorIdAsync(id);
            if (comanda == null) return NotFound();

            var resource = HateoasHelper.BuildResource(this, "Comandas", comanda, id);
            return Ok(resource);
        }

        // POST: Criar
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] Guid mesaId, [FromQuery] Guid garcomId)
        {
            var comanda = await _service.CriarAsync(mesaId, garcomId);
            return CreatedAtAction(nameof(GetById), new { id = comanda.Id }, comanda);
        }

        // PUT: Fechar comanda
        [HttpPut("{id}/fechar")]
        public async Task<IActionResult> Fechar(Guid id, [FromQuery] decimal valorTotal)
        {
            var comanda = await _service.FecharAsync(id, valorTotal);
            return comanda == null ? NotFound() : Ok(comanda);
        }

        // DELETE: Excluir
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sucesso = await _service.DeletarAsync(id);
            return !sucesso ? NotFound() : NoContent();
        }
    }
}
