using CineMaven.API.Configurations;
using CineMaven.API.Data;
using CineMaven.API.DTOs;
using CineMaven.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CineMaven.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private AppDbContext _context;

        public CategoriasController(
            AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> GetCategorias()
        {
            return await _context.Categorias
                .Select(c => new CategoriaResponseDTO()
                {
                    IdCategoria = c.IdCategoria,
                    NomeCategoria = c.NomeCategoria
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponseDTO>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if(categoria == null) 
                return NotFound();

            return new CategoriaResponseDTO
            {
                IdCategoria = categoria.IdCategoria,
                NomeCategoria = categoria.NomeCategoria
            };
        }


        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDTO>> PostCategoria(CategoriaDTO categoriaDTO)
        {
            var Categoria = new Categoria
            {
                NomeCategoria = categoriaDTO.Categoria
            };

            _context.Add(Categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = Categoria.IdCategoria },
            new CategoriaResponseDTO
            {
                IdCategoria = Categoria.IdCategoria,
                NomeCategoria = Categoria.NomeCategoria
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaResponseDTO>> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();
            
            string NomeCategoria = categoria.NomeCategoria.ToString();

            _context.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok($"A categoria '{NomeCategoria}' foi deletada com sucesso!");
        }
    }
}
