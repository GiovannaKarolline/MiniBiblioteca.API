using Microsoft.EntityFrameworkCore;
using MiniBiblioteca.API.Data;
using MiniBiblioteca.API.Models;
using MiniBiblioteca.API.Repositories.Interfaces;

namespace MiniBiblioteca.API.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private readonly BibliotecaContext _context;

        public LivroRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<List<Livro>> Listar()
        {
            return await _context.Livros.ToListAsync();
        }

        public async Task Adicionar(Livro livro) {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();
        }
    }
}
