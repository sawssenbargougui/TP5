using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.Services.MoviesApi.Services;
using AutoMapper;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper  _mapper;
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private readonly long _maxAllowedPosterSize = 1048576; // 1 MB

        public MoviesController(IMoviesService moviesService, IGenresService genresService, IMapper mapper)
        {
            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }

        // GET: api/Movies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound();

            var dto = _mapper.Map<MovieDetailsDto>(movie);
            return Ok(dto);
        }

        // GET: api/Movies/GetByGenreId
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync([FromQuery] int genreId)
        {
            var movies = await _moviesService.GetById(genreId);
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required.");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed.");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB.");

            var isValidGenre = await _genresService.IsvalidGenre(dto.GenderId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID.");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();
            await _moviesService.Add(movie);

            return Ok(movie);
        }

        // PUT: api/Movies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie found with ID {id}");

            var isValidGenre = await _genresService.IsvalidGenre(dto.GenderId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID.");

            if (dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed.");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB.");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenderId;
            movie.Year = dto.Year;
            movie.Storeline = dto.Storeligne;
            movie.Rate = dto.Rate;

            _moviesService.Update(movie);

            return Ok(movie);
        }

        // DELETE: api/Movies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie found with ID {id}");

             _moviesService.Delete(movie);
            return Ok(movie);
        }
    }
}
