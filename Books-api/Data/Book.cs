using System.ComponentModel.DataAnnotations.Schema;

namespace Books_api.Data
{[Table("tbl_Books")]
    public partial class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string image { get; set; }
        public double? Price { get; set; }

        public int? AuthorID { get; set; }

        public virtual Author Author { get; set; }

    }
}