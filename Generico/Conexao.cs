namespace Generico
{
    public class Conexao
    {
        public static string Conectar()
        {
            string stringConexao = "Data Source=KAWASAKI;User ID=sa;Password=paulino1234;Initial Catalog=AdminHouse;";
            return stringConexao;
        }
    }
}