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
        public IActionResult CreateGameListing(Game game, IFormFile img)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    // Adds the logged in user's ID to the object
                    game.OwnerID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    game.GameImage = GetByteArrayFromImage(img);
                    _context.Games.Add(game);
                    _context.SaveChanges();
                    return Redirect("/Home");
                }
                catch
                {
                    // Need to add error failed to save
                    return View(new ErrorViewModel { });
                }
            }
            return new UnauthorizedResult();
        }
        // Add to model or elsewhere
        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
