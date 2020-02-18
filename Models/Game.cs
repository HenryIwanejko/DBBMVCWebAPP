using System;
using System.Collections.Generic;

namespace DBBMVCWebApp.Models
{
    public class Game
    {
        public int GameID { get; set; }

        public string OwnerID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public byte[] GameImage { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public int Quantity { get; set; }
    }
}