using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DBBMVCWebApp.Data;
using DBBMVCWebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DBBMVCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Welcome";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
