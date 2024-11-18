using Microsoft.AspNetCore.Mvc;
using WebApplication3.Dtos;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresService.GetAll();
            return Ok(genres);
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] GenreDto dto)
        {
            var genre = new Genre
            {
                Name = dto.Name
            };
            await _genresService.Add(genre);
            return Ok(genre);
        }

        // PUT: api/Genres/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genre.Name = dto.Name;
            _genresService.Update(genre);

            return Ok(genre);
        }

        // DELETE: api/Genres/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

             _genresService.Delete(genre);
            return Ok(genre);
        }
    }
}
