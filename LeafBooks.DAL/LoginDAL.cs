using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Generico;
using System.Data.Common;
using LeafBooks.DTO;

namespace LeafBooks.DAL
{
    public class LoginDAL
    {
        string connectString = Conexao.Conectar();

        #region Insert

        public bool Login(string usuario, string senha)
        {
            LoginDTO dto = new LoginDTO();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("spselLogin", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user", usuario);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto.Login = reader["usuario"].ToString();
                            dto.Senha = reader["senha"].ToString();

                            if(dto.Login == usuario && dto.Senha == senha)
                            {
                                dto.validar = true;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    }
                }
            }
            return dto.validar;
        }

        //public bool Login(string usuario, string senha)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectString))
        //    {
        //        connection.Open();
        //        using (SqlCommand cmd = new SqlCommand("spInsLogin", connection))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@usuario", usuario);
        //            cmd.Parameters.AddWithValue("@senha", senha);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    return true;
        //}

        #endregion
    }
}
