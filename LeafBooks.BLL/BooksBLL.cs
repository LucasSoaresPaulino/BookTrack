using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeafBooks.DAL;
using LeafBooks.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace LeafBooks.BLL
{
    public class BooksBLL
    {
        public async void spInsBooks(string id, string nome, string imagem, string autor, string imgAutor, int tipo, string profile)
        {
            BooksDAL dal = new BooksDAL();
            dal.spInsBooks(id, nome, imagem, autor, imgAutor, tipo, profile);
        }

        public void spUpdImgBookItlg(string id, string login, string imagem)
        {
            BooksDAL dal = new BooksDAL();
            dal.spUpdImgBookItlg(id,login,imagem);
        }

        public List<BooksIndex> spSelBooksUser(string login)
        {
            BooksDAL dal = new BooksDAL();
            List<BooksIndex> dto = new List<BooksIndex>();
            dto = dal.spSelBooksUser(login);

            return dto;
        }

        public string spSelIdBookPage(string id)
        {
            BooksDAL dal = new BooksDAL();
            return dal.spSelIdBookPage(id);
        }

        public int spSelIdBookMarked(string id,string login)
        {
            BooksIndex dto = new BooksIndex();
            BooksDAL dal = new BooksDAL();

            dto.situacao = dal.spSelIdBookMarked(id,login);

            return dto.situacao;
        }

        public string spSelFraseBookId(string id)
        {
            BooksDAL dal = new BooksDAL();
            return dal.spSelFraseBookId(id);
        }

        public LivrosDTO spSelPhraseBook(string login)
        {
            LivrosDTO dto = new LivrosDTO();
            BooksDAL dal = new BooksDAL();
            dto.listFrase = dal.spSelPhraseBook(login);

            return dto;
        }

        public List<BooksIndex> spSelBooksCategUser(string login, int? categoria)
        {
            BooksDAL dal = new BooksDAL();
            List<BooksIndex> dto = new List<BooksIndex>();
            dto = dal.spSelBooksCategUser(login,categoria);

            return dto;
        }

        public List<BooksIndex> spSelSearchBooksUser (string login,string search, int? categoria)
        {
            if(categoria == null)
            {
                categoria = 1;
            }
            BooksDAL dal = new BooksDAL();
            List<BooksIndex> dto = new List<BooksIndex>();
            dto = dal.spSelSearchBooksUser(login, search, categoria);

            return dto;
        }

        public BooksIndex spSelBookForId(string id, string login)
        {
            BooksDAL dal = new BooksDAL();
            BooksIndex dto = new BooksIndex();
            dto = dal.spSelBookForId(id, login);

            return dto;
        }

        

        public ProfileBook spSelDadosProfile(string login)
        {
            ProfileBook dto = new ProfileBook();
            BooksDAL dal = new BooksDAL();
            dto = dal.spSelDadosProfile(login);

            return dto;
        }

        public List<ProfileBook> spSelMetaBooks(string login)
        {
            List<ProfileBook> dto = new List<ProfileBook>();
            BooksDAL dal = new BooksDAL();
            dto = dal.spSelMetaBooks(login);

            return dto;
        }

        #region save

        public void saveEditBook(string idBook,string name,string imagem,string autor,int ano, string[] marcadores, string login)
        {
            BooksDAL dal = new BooksDAL();
            dal.saveEditBook(idBook,name,imagem,autor,ano, marcadores,login);
        }

        public void spUpdMtsBooks(string id, int lido)
        {
            BooksDAL dal = new BooksDAL();
            dal.spUpdMtsBooks(id, lido);
        }
        public void spInsImgProfileBook(byte[] imagem,string login)
        {
            BooksDAL dal = new BooksDAL();
            dal.spInsImgProfileBook(imagem, login);
        }
        public void spInsFraseBook(string idBook,string texto)
        {
            BooksDAL dal = new BooksDAL();
            dal.spInsFraseBook(idBook, texto);
        }

        #endregion
    }
}
