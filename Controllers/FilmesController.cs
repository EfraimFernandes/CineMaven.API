using Microsoft.AspNetCore.Mvc;
using CineMaven.API.Data;
using CineMaven.API.DTOs;
using CineMaven.API.Models;
using CineMaven.API.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Options;

namespace CineMaven.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmesController : Controller
    {

        private AppDbContext _context;
        private readonly FilmesSettings _filmesSettings;

        public FilmesController(
            AppDbContext context,
            IOptions<FilmesSettings> filmesSettings)
        {
            _context = context;
            _filmesSettings = filmesSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmeResponseDTO>>> GetFilmes ()
        {
            return await _context.Filmes
                .Select(f => new FilmeResponseDTO
                {
                    IdFilme = f.IdFilme,
                    Titulo = f.Titulo,
                    Sinopse = f.Sinopse,
                    IdCategoria = f.IdCategoria,
                    AnoLancamento = f.AnoLancamento,
                    CapaUrl = f.CapaUrl
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FilmeResponseDTO>> GetFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);

            if (filme == null) return NotFound();

            return new FilmeResponseDTO
            {
                IdFilme = filme.IdFilme,
                Titulo = filme.Titulo,
                Sinopse = filme.Sinopse,
                IdCategoria = filme.IdCategoria,
                AnoLancamento = filme.AnoLancamento,
                CapaUrl = filme.CapaUrl
            };
        }

        [HttpPost]
        public async Task<ActionResult<FilmeResponseDTO>> PostFilme([FromForm] FilmeDTO filmeDto)
        {
            // Upload da imagem usando o caminho do appsettings
            string capaUrl = await SalvarCapa(filmeDto.Capa);

            var filme = new Filme
            {
                Titulo = filmeDto.Titulo,
                Sinopse = filmeDto.Sinopse,
                IdCategoria = filmeDto.IdCategoria,
                AnoLancamento = filmeDto.AnoLancamento,
                CapaUrl = capaUrl,
                DataAlta = DateTime.Now,
                UsuarioAlta = User.Identity?.Name ?? "sistema"
            };

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFilme), new { id = filme.IdFilme },
                new FilmeResponseDTO
                {
                    IdFilme = filme.IdFilme,
                    Titulo = filme.Titulo,
                    Sinopse = filme.Sinopse,
                    IdCategoria = filme.IdCategoria,
                    AnoLancamento = filme.AnoLancamento,
                    CapaUrl = filme.CapaUrl
                });
        }

        private async Task<string> SalvarCapa(IFormFile capa)
        {
            if (capa == null || capa.Length == 0)
                return string.Empty;

            // Usar o caminho do appsettings em vez do WebRootPath
            var uploadsFolder = _filmesSettings.BaseCaminhoImagem;

            // Criar diretório se não existir
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Nome único para o arquivo
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(capa.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await capa.CopyToAsync(stream);
            }

            // Retorna apenas o nome do arquivo ou um path relativo
            // Você precisará configurar um endpoint para servir essas imagens
            return filePath; // ou $"/capas/{fileName}" se configurar static files
        }

        private bool FilmeExists(int id)
        {
            return _context.Filmes.Any(e => e.IdFilme == id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FilmeResponseDTO>> AtualizarFilme(int id, [FromForm] FilmeUpdateDTO filmeDto)
        {
            try
            {
                // 1. Buscar o filme existente no banco
                var filmeExistente = await _context.Filmes.FindAsync(id);
                if (filmeExistente == null)
                {
                    return NotFound($"Não foi encontrado nenhum filme com o ID {id}");
                }

                // 2. Verificar se o ID na URL corresponde ao ID do DTO (se existir)
                if (filmeDto.IdFilme.HasValue && id != filmeDto.IdFilme.Value)
                {
                    return BadRequest("ID do filme não corresponde");
                }

                // 3. Atualizar a capa se uma nova imagem foi enviada
                if (filmeDto.Capa != null)
                {
                    // Deletar a imagem antiga (opcional)
                    if (!string.IsNullOrEmpty(filmeExistente.CapaUrl))
                    {
                        var caminhoAntigo = Path.Combine(_filmesSettings.BaseCaminhoImagem, filmeExistente.CapaUrl);
                        if (System.IO.File.Exists(caminhoAntigo))
                        {
                            System.IO.File.Delete(caminhoAntigo);
                        }
                    }

                    // Fazer upload da nova imagem
                    filmeExistente.CapaUrl = await SalvarCapa(filmeDto.Capa);
                }

                // 4. Atualizar as outras propriedades
                filmeExistente.Titulo = filmeDto.Titulo;
                filmeExistente.Sinopse = filmeDto.Sinopse;
                filmeExistente.IdCategoria = filmeDto.IdCategoria;
                filmeExistente.AnoLancamento = filmeDto.AnoLancamento;
                filmeExistente.DataAlta = DateTime.Now; // Atualizar data de modificação
                filmeExistente.UsuarioAlta = User.Identity?.Name ?? "sistema"; // Atualizar usuário

                // 5. Marcar como modificado e salvar
                _context.Entry(filmeExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // 6. Retornar o DTO de resposta
                return Ok(new FilmeResponseDTO
                {
                    IdFilme = filmeExistente.IdFilme,
                    Titulo = filmeExistente.Titulo,
                    Sinopse = filmeExistente.Sinopse,
                    IdCategoria = filmeExistente.IdCategoria,
                    AnoLancamento = filmeExistente.AnoLancamento,
                    CapaUrl = filmeExistente.CapaUrl
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmeExists(id))
                {
                    return NotFound($"Não foi encontrado nenhum filme com o ID {id}");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);

            if(filme == null)
                return NotFound($"Não foi possível encontrar um filme com o ID {id}");
                       
            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();

            return Ok($"O filme do ID {id} foi deletado com sucesso!");
        }


    }
}
