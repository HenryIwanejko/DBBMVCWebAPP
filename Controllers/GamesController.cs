using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DBBMVCWebApp.Data;
using DBBMVCWebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

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
            if (User.Identity.IsAuthenticated) {
                ViewData["Message"] = "MyGames";
                return View(_context.Games.Where(i => i.OwnerID == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList());
            }
            return Redirect("/Account/Login");
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
                    return Redirect("/Games/GamesForSale");
                }
                catch
                {
                    // Need to add error failed to save
                    return Redirect("Error");
                }
            }
            return new UnauthorizedResult();
        }
    
        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        public IActionResult View(int? id) 
        {
            if (User.Identity.IsAuthenticated) {
                Game game = _context.Games.Where(i => i.GameID == id).FirstOrDefault();
                ViewData["Message"] = game.Name;
                return View(game);
            }
            return Redirect("/Account/Login");
        }

        public IActionResult Edit(int? id) {
            if (id == null) {
                return Redirect("MyGames");
            }
            if (User.Identity.IsAuthenticated) {
                ViewData["Message"] = "Edit Game Infomation";
                Game game = _context.Games.Where(i => i.GameID == id).FirstOrDefault();
                return View(game);
            }
            return Redirect("/Account/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Game updatedGame) {
            if (User.Identity.IsAuthenticated) {
                Game retrievedGame = _context.Games.Where(x => x.GameID == updatedGame.GameID).FirstOrDefault();
                if (Request.Form.Files[0].Length > 0 && Request.Form.Files[0] != null) {
                    retrievedGame.GameImage = GetByteArrayFromImage(Request.Form.Files[0]);  
                }
                retrievedGame.Name = updatedGame.Name;
                retrievedGame.Description = updatedGame.Description;
                retrievedGame.Price = updatedGame.Price;
                retrievedGame.Quantity = updatedGame.Quantity;
                _context.SaveChanges();
                return Redirect("/Games/MyGames");
            }
            return Redirect("/Account/Login");
        }

        public IActionResult Delete(int? id) 
        {
            if (User.Identity.IsAuthenticated) {
                Game game = _context.Games.Where(i => i.GameID == id).FirstOrDefault();
                return View(game);
            }
            return Redirect("/Account/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Game chosenGame) 
        {
            if (User.Identity.IsAuthenticated) {
                Game game = _context.Games.Where(i => i.GameID == chosenGame.GameID).FirstOrDefault();
                _context.Games.Remove(game);
                _context.SaveChanges();
                return Redirect("/Games/MyGames");
            }
            return Redirect("/Account/Login");
        }

        public IActionResult Basket() 
        {   
            if (User.Identity.IsAuthenticated) 
            {
                ViewData["Message"] = "Basket";
                return View(_context.Games);
            }
            return Redirect("/Account/Login");
        }
    
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
