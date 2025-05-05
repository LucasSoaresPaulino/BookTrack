using LeafBooks.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LeafBooks.DTO;
using LeafBooks.BLL;
using ProjetoAdminCasa.Controllers;

namespace LeafBooks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Login(null, null);

            return View();
        }
        
        public bool Login(string login, string senha)
        {
            LoginDTO dto = new LoginDTO();
            LoginBLL bll = new LoginBLL();

            //dto.validar = bll.login(login, senha);
            login = "Paulino";

            HttpContext.Session.SetString("Login", login);

            return dto.validar;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}