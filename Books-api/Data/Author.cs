using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books_api.Data
{
    [Table("tbl_author")]
    public partial class Author
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }

        public virtual IList<Book> Books { get; set; }
    }
    }
