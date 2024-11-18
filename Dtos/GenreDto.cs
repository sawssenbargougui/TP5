using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Dtos
{
    public class GenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
