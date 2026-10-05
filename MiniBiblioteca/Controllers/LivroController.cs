using Microsoft.AspNetCore.Mvc;
using MiniBiblioteca.API.DTOs;
using MiniBiblioteca.API.Models;
using MiniBiblioteca.API.Services.Interfaces;

namespace MiniBiblioteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly ILivroService _service;

        public LivroController(ILivroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Livro>>> Listar()
        {
            return Ok(await _service.Listar());
        }

        [HttpPost]
        public async Task<ActionResult<Livro>> Criar(CriarLivroDTO dto)
        {
            Livro livro = await _service.Criar(dto);

            return Created($"api/livro/{livro.Id}", livro);
        }
    }
}
