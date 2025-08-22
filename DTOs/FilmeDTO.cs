namespace CineMaven.API.DTOs
{
    public class FilmeDTO
    {
        public string titulo {  get; set; } = string.Empty;
        public string sinopse {  get; set; } = string.Empty;
        public int idCategoria { get; set; } = 0;
        public int anoLancamento { get; set; } = 0;
        public IFormFile? Capa {  get; set; }
    }
}
