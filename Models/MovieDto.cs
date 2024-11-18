using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string  Title { get; set; }
        public  int Year { get; set; }
        public Double Rate  { get; set; }
        [MaxLength(2500)]
        public string  Storeligne { get; set; }
        public IFormFile?Poster { get; set; }
        public Byte GenderId{ get; set; }
    }
}
