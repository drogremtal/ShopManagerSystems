using Microsoft.AspNetCore.Mvc;
using ShopManagerSystems.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.DataLayer;

namespace ShopManagerSystems.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _DB;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext dBContext)
        {
            _logger = logger;
            _DB = dBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList()
        {
            var UsersList = _DB.User.ToList();
            return View(UsersList);
        }
        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateUser(User user) {

            _DB.User.Add(user);
            _DB.SaveChanges();
            return RedirectToAction("UserList");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}