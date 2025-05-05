using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using LeafBooks.DTO;
using LeafBooks.BLL;
using static LeafBooks.DTO.BooksDTO;
using static LeafBooks.DTO.BooksAux;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Page = LeafBooks.DTO.BooksDTO.Page;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace ProjetoAdminCasa.Controllers
{
    public class BooksController : Controller
    {
        #region View

        #region view
        public ActionResult Index()
        {
            HttpContext.Session.SetString("Login", "Paulino");

            string login = HttpContext.Session.GetString("Login");

            ProfileBook dto = new ProfileBook();
            BooksBLL bll = new BooksBLL();
            dto = bll.spSelDadosProfile(login);
            ViewBag.login = login;
            return View(dto);
        }

        public ActionResult Profile()
        {
            string login = HttpContext.Session.GetString("Login");

            BooksBLL bll = new BooksBLL();
            ProfileBook dto = new ProfileBook();

            dto = bll.spSelDadosProfile(login);
            var phrase = bll.spSelPhraseBook(HttpContext.Session.GetString("Login"));
            ViewBag.phrase = phrase;
            return View(dto);
        }

        public ActionResult AuthorsProfile()
        {
            string login = HttpContext.Session.GetString("Login");

            return View();
        }

        #endregion

        #region PartialView

        public ActionResult _PartialIndexBooks(int? categoria, int? pagina, string search)
        {
            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            ViewBag.categoria = categoria;

            List<BooksIndex> dto = new List<BooksIndex>();
            BooksBLL bll = new BooksBLL();
            string login = HttpContext.Session.GetString("Login");

            if (search != null)
            {
                dto = bll.spSelSearchBooksUser(login,search,categoria);
            }
            else
            {
                if (categoria == null)
                {
                    dto = bll.spSelBooksUser(login);
                }
                else
                {
                    dto = bll.spSelBooksCategUser(login, categoria);
                }
            }
            return PartialView(dto.ToPagedList(numeroPagina, tamanhoPagina));
        }

        public ActionResult _PartialEditBook(string id)
        {
            BooksIndex dto = new BooksIndex();
            BooksBLL bll = new BooksBLL();
            string login = HttpContext.Session.GetString("Login");

            dto = bll.spSelBookForId(id,login);
            return PartialView(dto);
        }

        public async Task<ActionResult> _PartialEditImgBook(string id,string search)
        {
            HttpClient client = new HttpClient();
            Item dto = new Item();
            Temperatures model = new Temperatures();

            if (search == null)
            {
                var bookResponse = await client.GetAsync($"https://www.googleapis.com/books/v1/volumes/" + id + "");
                var bookResult = bookResponse.Content.ReadAsStringAsync().Result;
                var ListId = JsonConvert.DeserializeObject<Item>(bookResult);

                dto = ListId;

                var AuthorResponse = await client.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=inauthor:" + dto.VolumeInfo.Authors[0] + "&maxResults=5&langRestrict=en");
                var AuthorResult = AuthorResponse.Content.ReadAsStringAsync().Result;
                var AuthorBooks = JsonConvert.DeserializeObject<Temperatures>(AuthorResult);
                model = AuthorBooks;
            }
            else
            {
                var bookResponse = await client.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=intitle:%22" + search + "%22&maxResults=5&langRestrict=en");
                var bookResult = bookResponse.Content.ReadAsStringAsync().Result;
                var ListId = JsonConvert.DeserializeObject<Temperatures>(bookResult);

                model = ListId;
            }
           
            return PartialView(model);
        }

        public ActionResult _PartialMetaBooks(int? pagina)
        {
            int tamanhoPagina = 12;
            int numeroPagina = pagina ?? 1;

            string login = HttpContext.Session.GetString("Login");
            List<ProfileBook> dto = new List<ProfileBook>();
            BooksBLL bll = new BooksBLL();
            dto = bll.spSelMetaBooks(login);
            return PartialView(dto.ToPagedList(numeroPagina, tamanhoPagina));
        }

        public ActionResult _PartialPhraseBook(int cont)
        {
            LivrosDTO dto = new LivrosDTO();
            BooksBLL bll = new BooksBLL();

            dto = bll.spSelPhraseBook(HttpContext.Session.GetString("Login"));
            ViewBag.cont = cont;

            return PartialView(dto);
        }

        public ActionResult _PartialOptionsSave(string id, string titulo, string imagem, string autor,string imgAutor)
        {
            BooksIndex dto = new BooksIndex();
            BooksBLL bll = new BooksBLL();

            List<string> listaDeStrings = new List<string>
            {
                id,titulo,imagem,autor,imgAutor
            };

            ViewBag.Info = listaDeStrings;
            dto.situacao = bll.spSelIdBookMarked(id, HttpContext.Session.GetString("Login"));

            return PartialView(dto);
        }

        #endregion

        #region ViewsAPI
        public async Task<Temperatures> BooksAPI(string name, int? page, int tipo,string endpoint)
        {
            Temperatures dto = new Temperatures();
            HttpClient client = new HttpClient();

            if (tipo == 0 || tipo == 1)
            {
                var booksResponse = await client.GetAsync($"{endpoint}");
                var booksResult = await booksResponse.Content.ReadAsStringAsync();
                dto = JsonConvert.DeserializeObject<Temperatures>(booksResult);
                if(tipo == 1)
                {
                    JObject jsonObject = JObject.Parse(booksResult);
                    dto.Volume.id = Convert.ToString(jsonObject["id"]);
                }
            }
            if(tipo == 2)
            {
                var AuthorResponse = await client.GetAsync($"{endpoint}");
                var AuthorResult = await AuthorResponse.Content.ReadAsStringAsync();
                dto = JsonConvert.DeserializeObject<Temperatures>(AuthorResult);
            }
            return dto;
        }

        public async Task<WikipediaResponse> WikiAPI(string autor,string tipo)
        {
            WikipediaResponse dto = new WikipediaResponse();
            HttpClient client = new HttpClient();
            try
            {
                var wikiResponse = await client.GetAsync($"https://pt.wikipedia.org/w/api.php?action=query&format=json&titles=" + autor + "&prop=pageimages|extracts&pithumbsize=300&pilimit=1&exintro&explaintext&redirects=1");
                var wikiResult = wikiResponse.Content.ReadAsStringAsync().Result;
                dto = JsonConvert.DeserializeObject<WikipediaResponse>(wikiResult);
            }
            catch
            {
                var wikiResponse = await client.GetAsync($"https://en.wikipedia.org/w/api.php?action=query&format=json&titles=" + autor + "&prop=pageimages|extracts&pithumbsize=300&pilimit=1&exintro&explaintext&redirects=1");
                var wikiResult = wikiResponse.Content.ReadAsStringAsync().Result;
                dto = JsonConvert.DeserializeObject<WikipediaResponse>(wikiResult);
            }
            return dto;
        }

        public async Task<ActionResult> SearchBooks(string name, int? pagina)
        {
            ViewBag.pesquisa = name;
            int tamanhoPagina = 20;
            int numeroPagina = pagina ?? 1;

            autorDesc aut = new autorDesc();
            HttpClient client = new HttpClient();
            Temperatures dto = new Temperatures();
            WikipediaResponse wikiDTO = new WikipediaResponse();

            dto = await BooksAPI(name, tamanhoPagina, 0, "https://www.googleapis.com/books/v1/volumes?q="+name+"&maxResults=20&startIndex=" + tamanhoPagina);
            wikiDTO = await WikiAPI(dto.Items[0].VolumeInfo.Authors[0],"pt");

            foreach (var item in wikiDTO.Query.Pages)
            {
                Page validate = item.Value;

                if (validate.Extract == null)
                {
                    wikiDTO = await WikiAPI(dto.Items[0].VolumeInfo.Authors[0], "en");
                }
            }

            string imagem = "";

            foreach (var pageKeyValue in wikiDTO.Query.Pages)
            {
                if (pageKeyValue.Value.Thumbnail != null)
                {
                    imagem = pageKeyValue.Value.Thumbnail.Source;
                }
            }

            aut.imagem = imagem;
            ViewBag.AutorInfo = aut;

            return View(dto);
        }

        public async Task<ActionResult> pageBook(string id, string endpoint)
        {
            HttpClient client = new HttpClient();
            Temperatures dto = new Temperatures();
            Temperatures model = new Temperatures();
            WikipediaResponse wikiDTO = new WikipediaResponse();
            autorDesc aut = new autorDesc();
            BooksBLL bll = new BooksBLL();

            dto = await BooksAPI(null,null,1, "https://www.googleapis.com/books/v1/volumes/" + id + "");

            if (dto.Volume.Description != null)
            {
                dto.Volume.Description = RemoverAtributosHtml(dto.Volume.Description);
            }
            var AuthorBooks = await BooksAPI(null, null, 2, "https://www.googleapis.com/books/v1/volumes?q=inauthor:" + dto.Volume.Authors[0] + "&maxResults=20");

            int a = 0;
            string desc = "";
            List<string> minhaLista = new List<string>();
            List<string> listaId = new List<string>();

            var validated = bll.spSelIdBookPage(id);
            var frase = bll.spSelFraseBookId(id);
            ViewBag.validate = validated;
            ViewBag.frase = frase;

            try
            {
                for (int i = 0; i <= 4;)
                {
                    if (AuthorBooks.Items[a].VolumeInfo.ImageLinks != null && AuthorBooks.Items[i].VolumeInfo.Title != AuthorBooks.Items[a].VolumeInfo.Title)
                    {
                        minhaLista.Add(AuthorBooks.Items[a].VolumeInfo.ImageLinks.Thumbnail);
                        listaId.Add(AuthorBooks.Items[a].Id);
                        desc = desc + ", " + AuthorBooks.Items[a].VolumeInfo.Title;
                        i++;
                    }
                    a++;
                    if (a == 10)
                    {
                        break;
                    }
                }
                ViewBag.Autor = minhaLista;
                ViewBag.AuthorBid = listaId;


                if (dto.Volume.Authors[0].Contains(".") || dto.Volume.Authors[0].Contains(","))
                {
                    dto.Volume.Authors[0] = dto.Volume.Authors[0].Replace(".", ". ");
                    dto.Volume.Authors[0] = dto.Volume.Authors[0].Replace(",", "");
                }
               
                wikiDTO = await WikiAPI(dto.Volume.Authors[0], "pt");
                foreach (var item in wikiDTO.Query.Pages)
                {
                    Page validate = item.Value;

                    if (validate.Extract == null)
                    {
                        wikiDTO = await WikiAPI(dto.Volume.Authors[0], "en");
                    }
                }

                string descricao = "";
                string imagem = "";

                foreach (var pageKeyValue in wikiDTO.Query.Pages)
                {
                    Page pagina = pageKeyValue.Value;
                    descricao = pagina.Extract;
                    if (pagina.Thumbnail != null)
                    {
                        imagem = pagina.Thumbnail.Source;
                    }
                    Console.WriteLine($"Extract: {pagina.Extract}");
                }

                descricao = RemoverAtributosHtml(descricao);

                aut.descricao = descricao;
                aut.imagem = imagem;
                ViewBag.AutorInfo = aut;

                return View(dto);
            }
            catch
            {
                if (dto.Volume.Genero != null)
                {
                    aut.descricao = dto.Volume.Authors[0] + ", escritor(a) cujas grandes obras estão no titulo literário: " + dto.Volume.Genero[0] + ". " + dto.Volume.Authors[0] + " cativa seus leitores com narrativas imersivas, proporcionando aos leitores uma experiência única em cada uma de suas obras. Alguma de suas obras são: " + desc + "";
                }

                if (dto.Volume.Genero == null)
                {
                    aut.descricao = dto.Volume.Authors[0] + ", escritor(a) cujas grandes obras cativa seus leitores com narrativas imersivas, proporcionando aos leitores uma experiência única em cada uma de suas obras. Alguma de suas obras são: " + desc + "";
                }

                aut.imagem = "";
                ViewBag.AutorInfo = aut;

                return View(dto);
            }      
        }

        public async Task<ActionResult> InfoAuthor(string nome, int? paginas)
        {
            int? page = 0;

            if (paginas <= 4)
            {
                page = paginas * 20;
            }

            ViewBag.autor = nome;

            HttpClient client = new HttpClient();
            WikipediaResponse wikiDTO = new WikipediaResponse();
            Temperatures dto = new Temperatures();
            autorDesc aut = new autorDesc();

            var AuthorResponse = await client.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=inauthor:" + nome + "&maxResults=18&startIndex=" + page + "");
            var AuthorResult = AuthorResponse.Content.ReadAsStringAsync().Result;
            var AuthorBooks = JsonConvert.DeserializeObject<Temperatures>(AuthorResult);

            dto = await BooksAPI(null,page,2, "https://www.googleapis.com/books/v1/volumes?q=inauthor:" + nome + "&maxResults=18&startIndex=" + page + "");

            try
            {
                if (nome.Contains(".") || nome.Contains(","))
                {
                    nome = nome.Replace(".", ". ");
                    nome = nome.Replace(",", "");
                }

                wikiDTO = await WikiAPI(dto.Items[0].VolumeInfo.Authors[0], "pt");
                foreach (var item in wikiDTO.Query.Pages)
                {
                    Page validate = item.Value;

                    if (validate.Extract == null)
                    {
                        wikiDTO = await WikiAPI(dto.Volume.Authors[0], "en");
                    }
                }
                foreach (var pageKeyValue in wikiDTO.Query.Pages)
                {
                    Page pagina = pageKeyValue.Value;
                    aut.descricao = pagina.Extract;
                    if (pagina.Thumbnail != null)
                    {
                        aut.imagem = pagina.Thumbnail.Source;
                    }
                    Console.WriteLine($"Extract: {pagina.Extract}");
                }

                aut.descricao = RemoverAtributosHtml(aut.descricao);

                ViewBag.AutorInfo = aut;

                return View(dto);
            }
            catch
            {
                aut.descricao = nome + ", escritor(a) cujas grandes obras cativa seus leitores com narrativas imersivas, proporcionando aos leitores uma experiência única em cada uma de suas obras.";
                aut.imagem = "";
                ViewBag.AutorInfo = aut;

                return View(dto);
            }
        }

        public static string RemoverAtributosHtml(string html)
        {
            string variavel = Regex.Replace(html, "<[^<>]+?>", string.Empty);
            variavel = variavel.Replace("\n", " ");

            return variavel;
        }
        #endregion

        #endregion

        #region Save

        public async void saveBook(string id,string imagem, string autor,string imgAutor, int tipo)
        {
            // Captura o valor da sessão o mais cedo possível
            var login = HttpContext.Session.GetString("Login");

            // Cria a instância da camada de negócio
            BooksBLL bll = new BooksBLL();

            // Monta a URL corretamente
            string url = $"https://www.googleapis.com/books/v1/volumes/{id}";

            // Chama a API
            var API = await BooksAPI(null, null, 1, url);

            // Extrai o nome do livro
            var bookName = API.Volume.Title;

            // Limpa a string da imagem
            imagem = imagem.Replace("amp", "").Replace(";", "");

            // Salva no banco via BLL
            bll.spInsBooks(id, bookName, imagem, autor, imgAutor, tipo, login);
        }

        public async void testeNewBook(string id,string imagem, string autor, string imgAutor, int tipo)
        {
            string login = HttpContext.Session.GetString("Login");

            HttpClient client = new HttpClient();
            BooksBLL bll = new BooksBLL();

            Item dto = new Item();
            
            var bookResponse = await client.GetAsync($"https://www.googleapis.com/books/v1/volumes/" + id + "");
            var bookResult = bookResponse.Content.ReadAsStringAsync().Result;
            var ListId = JsonConvert.DeserializeObject<Item>(bookResult);
            dto = ListId;

            string nome = dto.VolumeInfo.Title;
            bll.spInsBooks(id, nome, imagem, autor, imgAutor, tipo,login);
        }

        public async Task<string> saveImgInteligent(string nome, string id,int? tipo)
        {
            BooksBLL bll = new BooksBLL();
            string login = HttpContext.Session.GetString("Login");
            string image = "";

            if (tipo != 1)
            {
                Task<string> asyncTask = imgBookApiName(nome);
                image = await asyncTask;
            }
            else
            {
                image = nome;
            }

            if (image != "error")
            {
                bll.spUpdImgBookItlg(id, login, image);
            }
            return image;
        }

        public void saveImgProfile(IFormFile imagem)
        {
            string login = HttpContext.Session.GetString("Login");

            byte[] file = null;
            if (imagem != null)
            {
                file = genBytes(imagem);
            }
            BooksBLL bll = new BooksBLL();
            bll.spInsImgProfileBook(file,login);
        }

        public void SavePrefBook(string idBook, string name, string imagem,string autor,int ano,string tipo)
        {
            string login = HttpContext.Session.GetString("Login");

            BooksBLL bll = new BooksBLL();

            string[] marcadores = tipo.Split(',');
            for(int i = 0; i <= marcadores.Length-1; i++)
            {
                marcadores[i] = Regex.Replace(marcadores[i], @"[^\d]", ""); ;
            }
            imagem = imagem.Replace("amp", "");

            bll.saveEditBook(idBook, name,imagem,autor,ano,marcadores,login);
        }

        public void livroLido(string id)
        {
            BooksBLL bll = new BooksBLL();
            bll.spUpdMtsBooks(id, 1);
        }

        public void saveFraseBook(string idBook,string texto)
        {
            BooksBLL bll = new BooksBLL();
            bll.spInsFraseBook(idBook,texto);
        }

        #endregion

        #region Formatação

        public async Task<string> imgBookApiName(string nome)
        {
            string imagem = "";
            try
            {
                HttpClient client = new HttpClient();
                Temperatures model = new Temperatures();
                var BookResponse = await client.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=intitle:%22" + nome + "%22&maxResults=1&langRestrict=en");
                var BookResult = BookResponse.Content.ReadAsStringAsync().Result;
                var BookCompleted = JsonConvert.DeserializeObject<Temperatures>(BookResult);

                imagem = BookCompleted.Items[0].VolumeInfo.ImageLinks.Thumbnail;

                return imagem;
            }
            catch
            {
                imagem = "error";
                return imagem;
            }

        }
        public void changeAccount(string login)
        {
            HttpContext.Session.SetString("Login", login);
        }
        public byte[] genBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        #endregion

    }
}
