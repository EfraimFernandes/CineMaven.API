using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CineMaven.API.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario {  get; set; } = 0;
        public string codUsuario { get; set; } = string.Empty;
        public string senha {  get; set; } = string.Empty;
        public string nome {  get; set; } = string.Empty;
        public string email { get; set;} = string.Empty;
        public int ativo { get; set; } = 0;
    }
}
