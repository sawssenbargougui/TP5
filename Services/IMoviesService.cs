using WebApplication3.Models;

namespace WebApplication3.Services
{
    public interface IMoviesService
    {

        Task<IEnumerable<Movie>> GetAll(byte genreId = 0);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);
        Task GetByGenreId(int genreId);
    }
}
