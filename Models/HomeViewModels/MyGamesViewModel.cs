using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DBBMVCWebApp.Data;
using System.Threading.Tasks;

namespace DBBMVCWebApp.Models.HomeViewModels
{
    public class MyGamesViewModel
    {
        private ApplicationDbContext _context;

        public MyGamesViewModel(ApplicationDbContext context) 
        {
            _context = context;
        }

        public IEnumerable<Game> RetrieveUsersGames(string ownerId) {
            return _context.Games.Where(i => i.OwnerID == ownerId).ToList();
        }
    }
}
