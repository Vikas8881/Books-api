using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books_UI.Static
{
    public static class EndPoints
    {
        public static string BaseUrl = "https://localhost:44324/";
        public static string AuthorsEndpoint = $"{BaseUrl}api/authors/";
        public static string BooksEndpoint = $"{BaseUrl}api/books/";
    }
}
