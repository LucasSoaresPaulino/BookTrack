using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using Generico;
using LeafBooks.DTO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace LeafBooks.DAL
{
    public class BooksDAL
    {
        string connectString = Conexao.Conectar();

        public void spInsBooks(string id, string nome, string imagem, string autor, string imgAutor, int tipo, string profile)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsBooks", connection))
                {
                    if (profile != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@imagem", imagem);
                        cmd.Parameters.AddWithValue("@autor", autor);
                        cmd.Parameters.AddWithValue("@imgAutor", imgAutor);
                        cmd.Parameters.AddWithValue("@tipo", tipo);
                        cmd.Parameters.AddWithValue("@profile", profile);
                        cmd.Parameters.AddWithValue("@data", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spUpdImgBookItlg(string id, string login, string imagem)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdImgBookItlg", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idBook", id);
                    cmd.Parameters.AddWithValue("@user", login);
                    cmd.Parameters.AddWithValue("@imagem", imagem);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string spSelIdBookPage(string id)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelIdBookPage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idBook", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader["idBook"].ToString();
                        }
                    }
                }
            }
            return result;
        }

        public int spSelIdBookMarked(string id, string login)
        {
            int result =0;
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelIdBookMarked", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idBook", id);
                    command.Parameters.AddWithValue("@user", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["situacao"]);
                        }
                    }
                }
            }
            return result;
        }

        public string spSelFraseBookId(string id)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelFraseBookId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idBook", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader["frase"].ToString();
                        }
                    }
                }
            }
            return result;
        }

        public List<PhraseBook> spSelPhraseBook(string login)
        {
            List<PhraseBook> result = new List<PhraseBook>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelPhraseBook", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@user", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PhraseBook dto = new PhraseBook();

                            dto.frase = reader["frase"].ToString();
                            dto.livro = reader["nome"].ToString();
                            //dto.autor = reader["frase"].ToString();

                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<BooksIndex> spSelBooksUser(string login)
        {
            List<BooksIndex> result = new List<BooksIndex>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelBooksUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@user", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BooksIndex dto = new BooksIndex();

                            dto.id = reader["idBook"].ToString();
                            dto.nome = reader["nome"].ToString();
                            dto.imagem = reader["imagem"].ToString();
                            dto.autor = reader["autor"].ToString();
                            dto.situacao = Convert.ToInt32(reader["situacao"]);
                            dto.dataInclusao = Convert.ToDateTime(reader["dataInclusao"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<BooksIndex> spSelBooksCategUser(string login, int? categoria)
        {
            List<BooksIndex> result = new List<BooksIndex>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelBooksCategUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@user", login);
                    command.Parameters.AddWithValue("@categoria", categoria);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BooksIndex dto = new BooksIndex();

                            dto.id = reader["idBook"].ToString();
                            dto.nome = reader["nome"].ToString();
                            dto.imagem = reader["imagem"].ToString();
                            dto.autor = reader["autor"].ToString();
                            dto.situacao = Convert.ToInt32(reader["situacao"]);
                            dto.dataInclusao = Convert.ToDateTime(reader["dataInclusao"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<BooksIndex> spSelSearchBooksUser(string login, string search, int? categoria)
        {
            List<BooksIndex> result = new List<BooksIndex>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelSearchBooksUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@user", login);
                    command.Parameters.AddWithValue("@search", search);
                    command.Parameters.AddWithValue("@categoria", categoria);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BooksIndex dto = new BooksIndex();

                            dto.id = reader["idBook"].ToString();
                            dto.nome = reader["nome"].ToString();
                            dto.imagem = reader["imagem"].ToString();
                            dto.autor = reader["autor"].ToString();
                            dto.situacao = Convert.ToInt32(reader["situacao"]);
                            dto.dataInclusao = Convert.ToDateTime(reader["dataInclusao"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public BooksIndex spSelBookForId(string id, string login)
        {
            BooksIndex result = new BooksIndex();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelBookForId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idBook", id);
                    command.Parameters.AddWithValue("@user", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            BooksIndex dto = new BooksIndex();
                            dto.id = reader["idBook"].ToString();
                            dto.nome = reader["nome"].ToString();
                            dto.imagem = reader["imagem"].ToString();
                            dto.situacao = Convert.ToInt32(reader["situacao"]);
                            dto.dataInclusao = Convert.ToDateTime(reader["dataInclusao"]);
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public ProfileBook spSelDadosProfile(string login)
        {
            ProfileBook result = new ProfileBook();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelDadosProfile", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@login", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ProfileBook dto = new ProfileBook();
                            dto.nome = reader["nome"].ToString();
                            if (!reader["imagem"].Equals(DBNull.Value))
                                dto.imagem = Convert.ToBase64String((byte[])reader["imagem"]);
                            dto.idBook = reader["idBook"].ToString();
                            dto.nomeLivro = reader["nomeLivro"].ToString();
                            dto.imgBook = reader["imgBook"].ToString();
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public List<ProfileBook> spSelMetaBooks(string login)
        {
            List<ProfileBook> result = new List<ProfileBook>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelMetaBooks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@user", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProfileBook dto = new ProfileBook();

                            dto.nome = reader["nome"].ToString();
                            dto.idBook = reader["idBook"].ToString();
                            dto.imgBook = reader["imagem"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }
        #region save

        public void saveEditBook(string idBook, string name, string imagem, string autor,int ano, string[] marcadores, string login)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdBook", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var item in marcadores)
                    {
                        cmd.Parameters.AddWithValue("@idBook", idBook);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@imagem", imagem);
                        cmd.Parameters.AddWithValue("@autor", autor);
                        cmd.Parameters.AddWithValue("@anoLeitura", ano);
                        cmd.Parameters.AddWithValue("@user", login);
                        cmd.Parameters.AddWithValue("@marcadores", item);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spUpdMtsBooks(string id, int lido)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdMtsBooks", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idBook", id);
                    cmd.Parameters.AddWithValue("@lido", lido);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsImgProfileBook(byte[] imagem, string login)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsImgProfileBook", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@imagem", imagem);
                    cmd.Parameters.AddWithValue("@user", login);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsFraseBook(string idBook, string texto)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsFraseBook", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idBook", idBook);
                    cmd.Parameters.AddWithValue("@texto", texto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}
