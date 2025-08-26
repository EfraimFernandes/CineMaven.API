using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CineMaven.API.Models
{
    public class Filme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFilme { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public string Sinopse { get; set; } = string.Empty;
        public int IdCategoria { get; set; } = 0;
        public int AnoLancamento { get; set; } = 0;
        public string CapaUrl { get; set; } = string.Empty;
        public DateTime? DataAlta { get; set; }
        public string UsuarioAlta {  get; set; } = string.Empty;

    }
}
