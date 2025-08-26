namespace CineMaven.API.DTOs
{
    public class FilmeResponseDTO
    {
        public int IdFilme { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Sinopse { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public int AnoLancamento { get; set; }
        public string CapaUrl { get; set; } = string.Empty;
    }
}