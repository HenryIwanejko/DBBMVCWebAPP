using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DBBMVCWebApp.Data;
using DBBMVCWebApp.Models;

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
            ViewData["Message"] = "Games for sale";
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

        public IActionResult CreateGameListing()
        {
            ViewData["Message"] = "Create Game Listing";
            return View();
        }

        [HttpPost, ActionName("CreateGameListing")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateGameListing(Game game)
        {
            try
            {
                _context.Games.Add(game);
                _context.SaveChanges();
                return Redirect("/Home");
            }
            catch
            {
                return View(new ErrorViewModel { });
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
