using System;
using System.Collections.Generic;

namespace DBBMVCWebApp.Models
{
    public class BasketItem
    {
        public int GameID { get; set; }

        public string OwnerID { get; set; }

        public int PurchaseQuantity { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public byte[] GameImage { get; set; }
        
        public BasketItem(int gameId, string ownerId, int purchaseQuantity, string name, string description, decimal price, byte[] gameImage)
        {
                GameID = gameId;
                OwnerID = ownerId;
                PurchaseQuantity = purchaseQuantity;
                Name = name;
                Description = description;
                Price = price;
                GameImage = gameImage;
        }
    }
    
}