namespace MiniBiblioteca.API.Models
{
    public class Livro
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = "";
        public string ISBN { get; set; } = "";
    }
}
