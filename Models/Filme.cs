namespace CineMaven.API.Models
{
    public class Filme
    {
        public string titulo { get; set; } = string.Empty;
        public string sinopse { get; set; } = string.Empty;
        public int idCategoria { get; set; } = 0;
        public int anoLancamento { get; set; } = 0;
        public string CapaUrl { get; set; } = string.Empty;
        public DateTime? dataAlta { get; set; };
        public string usuarioAlta {  get; set; } = string.Empty;

    }
}
