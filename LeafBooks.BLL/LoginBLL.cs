using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeafBooks.DAL;
using LeafBooks.DTO;

namespace LeafBooks.BLL
{
    public class LoginBLL
    {
        public bool login(string login, string senha)
        {
            LoginDTO dto = new LoginDTO();
            LoginDAL dal = new LoginDAL();

            dto.validar = dal.Login(login, senha);

            return dto.validar;
        }
    }
}
