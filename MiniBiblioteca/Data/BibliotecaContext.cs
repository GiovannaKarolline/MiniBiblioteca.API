using Microsoft.EntityFrameworkCore;
using MiniBiblioteca.API.Models;

namespace MiniBiblioteca.API.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        public DbSet<Livro> Livros { get; set; }
    }
}
