using System;
using System.Linq;
using System.Collections.Generic;
using DBBMVCWebApp.Data;

namespace DBBMVCWebApp.Models.HomeViewModels
{
    public class MyGameViewModel
    {
        private readonly ApplicationDbContext _context;

        public MyGameViewModel(ApplicationDbContext context) {
            _context = context;
        }
    
        public IEnumerable<Game>  RetrieveUsersGames(string ownerId) {
            return _context.Games.Where(game => game.OwnerID == ownerId);
        }
    }
}       