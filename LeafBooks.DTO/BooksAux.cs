using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafBooks.DTO
{
    public class BooksAux
    {
        public class testeWiki
        {
            public Query Query { get; set; }
        }

        public class Query
        {
            public Dictionary<string, Pagee> Pages { get; set; }
        }

        public class Pagee
        {
            public ImageInfo[] ImageInfo { get; set; }
        }

        public class ImageInfo
        {
            public string Url { get; set; }
        }
    }
}
