using MiniBiblioteca.API.DTOs;
using MiniBiblioteca.API.Models;
using MiniBiblioteca.API.Repositories.Interfaces;
using MiniBiblioteca.API.Services.Interfaces;

namespace MiniBiblioteca.API.Services
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _repository;

        public LivroService(ILivroRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Livro>> Listar()
        {
            return _repository.Listar();
        }

        public async Task<Livro> Criar(CriarLivroDTO dto)
        {
            var livro = new Livro
            {
                Titulo = dto.Titulo,
                ISBN = dto.ISBN
            };

            await _repository.Adicionar(livro);

            return livro;
        }
    }
}
