using System.ComponentModel.DataAnnotations;

namespace CineMaven.API.DTOs
{
    public class FilmeUpdateDTO
    {
        public int? IdFilme { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A sinopse é obrigatória")]
        public string Sinopse { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma categoria válida")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "O ano de lançamento é obrigatório")]
        [Range(1900, 2100, ErrorMessage = "Ano de lançamento inválido")]
        public int AnoLancamento { get; set; }

        public IFormFile? Capa { get; set; }
    }
}