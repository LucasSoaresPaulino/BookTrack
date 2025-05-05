using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LeafBooks.DTO
{
    public class BooksDTO
    {
        #region API

        public partial class Temperatures
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("items")]
            public List<Item> Items { get; set; }
            public List<VolumeInfo> book { get; set; }
            [JsonProperty("volumeInfo")]
            public VolumeInfo Volume { get; set; }
        }

        public partial class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("volumeInfo")]
            public VolumeInfo VolumeInfo { get; set; }
        }

        public class VolumeInfo
        {
            public string id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("authors")]
            public string[] Authors { get; set; }

            [JsonProperty("categories")]
            public string[] Genero { get; set; }

            [JsonProperty("publisher")]
            public string Publisher { get; set; }

            [JsonProperty("publishedDate")]
            public string PublishedDate { get; set; } = "";

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("pageCount")]
            public long PageCount { get; set; }

            [JsonProperty("language")]
            public string lingua { get; set; }

            [JsonProperty("imageLinks")]
            public ImageLinks ImageLinks { get; set; }
        }

        public partial class ImageLinks
        {
            [JsonProperty("thumbnail")]
            public string Thumbnail { get; set; }
        }

        public class WikipediaResponse
        {
            public Query Query { get; set; }
        }

        public class Query
        {
            public Dictionary<string, Page> Pages { get; set; }
        }

        public class Page
        {
            public string Extract { get; set; }

            [JsonProperty("thumbnail")]
            public Thumbnail Thumbnail { get; set; }
        }

        public class Thumbnail
        {
            public string Source { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public class autorDesc
        {
            public string descricao { get; set; }
            public string imagem { get; set; }

        }
        #endregion
    }

    public class LivrosDTO
    {
        public List<BooksIndex> listBooks { get; set; }
        public List<PhraseBook> listFrase { get; set; }
        public List<MetaBook> listMeta { get; set; }
    }

    public class ProfileBook
    {
        public string nome { get; set; }
        public string imagem { get; set; }
        public string idBook { get; set; }
        public string nomeLivro { get; set; }
        public string imgBook { get; set; }
    }

    public class BooksIndex
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string imagem { get; set; }
        public string autor { get; set; }
        public int situacao { get; set; }
        public DateTime dataInclusao { get; set; }
    }

    public class PhraseBook
    {
        public string frase { get; set; }
        public string livro { get; set; }
        public string autor { get; set; }
    }

    public class MetaBook
    {

    }
}
