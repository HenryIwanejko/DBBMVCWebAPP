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
            if (User.Identity.IsAuthenticated) {
                ViewData["Message"] = "Games for sale";
                return View(_context.Games);
            }
            return Redirect("/Account/Login");
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
            if (User.Identity.IsAuthenticated) {
                ViewData["Message"] = "Create Game Listing";
                return View();
            }
            return Redirect("/Account/Login");
        }

        [HttpPost, ActionName("CreateGameListing")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateGameListing(Game game)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    // Adds the logged in user's ID to the object
                    game.OwnerID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    game.GameImage = GetByteArrayFromImage(Request.Form.Files[0]);
                    _context.Games.Add(game);
                    _context.SaveChanges();
                    return Redirect("/Home");
                }
                catch
                {
                    // Need to add error failed to save
                    return Redirect("Error");
                }
            }
            return new UnauthorizedResult();
        }

        public IActionResult MyGames() 
        {
            if (User.Identity.IsAuthenticated) 
            {
                var model =  new Models.HomeViewModels.MyGameViewModel(_context);
                ViewData["Message"] = "MyGames";
                return View(model.RetrieveUsersGames(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            return Redirect("/Account/Login");
        }

        // TODO: Add to model 
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
