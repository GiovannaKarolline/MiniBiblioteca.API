using System.ComponentModel.DataAnnotations;

namespace MiniBiblioteca.API.DTOs
{
    public class CriarLivroDTO
    {
        [Required]
        public string Titulo { get; set; } = "";

        [Required]
        public string ISBN { get; set; } = "";
    }
}
