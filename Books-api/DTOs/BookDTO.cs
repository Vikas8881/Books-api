using Books_api.Data;
using System;
using System.Collections.Generic;
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
        public double? Price { get; set; }

        public int? AuthorID { get; set; }

        public virtual AuthorDTO Author { get; set; }
    }
}
