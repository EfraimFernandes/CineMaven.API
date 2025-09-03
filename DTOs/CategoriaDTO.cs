using System.ComponentModel.DataAnnotations;

namespace CineMaven.API.DTOs
{
    public class CategoriaDTO
    {
        [Required(ErrorMessage = "A Categoria é obrigatória")]
        [StringLength(100, ErrorMessage = "A Categoria deve ter no máximo 100 caracteres")]
        public string Categoria { get; set; } = string.Empty;
    }
}
