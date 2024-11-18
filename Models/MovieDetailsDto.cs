using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public int Year { get; set; }
        public Double Rate { get; set; }

        public string Storeligne { get; set; }
        public byte[] Poster { get; set; }
        public Byte GenderId { get; set; }
        public string  GenderName { get; set; }
    }
}


