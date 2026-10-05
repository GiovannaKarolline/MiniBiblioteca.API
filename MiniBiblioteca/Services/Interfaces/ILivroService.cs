using MiniBiblioteca.API.DTOs;
using MiniBiblioteca.API.Models;

namespace MiniBiblioteca.API.Services.Interfaces
{
    public interface ILivroService
    {
        Task<List<Livro>> Listar();

        Task<Livro> Criar(CriarLivroDTO dto);
    }
}
