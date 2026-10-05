using MiniBiblioteca.API.Models;

namespace MiniBiblioteca.API.Repositories.Interfaces
{
    public interface ILivroRepository
    {
        Task<List<Livro>> Listar();

        Task Adicionar(Livro livro);
    }
}
