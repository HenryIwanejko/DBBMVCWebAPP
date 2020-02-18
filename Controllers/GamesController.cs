using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DBBMVCWebApp.Data;
using DBBMVCWebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DBBMVCWebApp.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult GamesForSale()
        {
            if (User.Identity.IsAuthenticated) {
                ViewData["Message"] = "Games for sale";
                return View(_context.Games);
            }
            return Redirect("/Account/Login");
        }
    
        public IActionResult MyGames() 
        {
            Models.HomeViewModels.MyGamesViewModel myGamesViewModel =  new Models.HomeViewModels.MyGamesViewModel(_context);
            ViewData["Message"] = "MyGames";
            return View(myGamesViewModel.RetrieveUsersGames(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
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
