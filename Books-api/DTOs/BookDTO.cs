using Books_api.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books_api.DTOs
{
    public class BookDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string image { get; set; }
        public decimal? Price { get; set; }

        public int? AuthorID { get; set; }

        public virtual AuthorDTO Author { get; set; }
    }
    public class BookCreateDto
    {
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        [Required]
        public string ISBN { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        public string image { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public int AuthorID { get; set; }

    }
    public class BookUpdateDTO
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        [Required]
        [StringLength(500)]
        public string Summary { get; set; }
        public string image { get; set; }
        public decimal? Price { get; set; }
        public int? AuthorId { get; set; } 
        // Missing from the previous lesson. Ensure that you include.

    }
}
