using System;
using System.Collections.Generic;

namespace DBBMVCWebApp.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }

        public int OrderID { get; set; }

        public int GameID { get; set; }

        public int Quantity { get; set; }

        public virtual Game Game { get; set; }

        public virtual Order Order { get; set; }

        public OrderItem() {}
        
        public OrderItem(int orderID, int gameID, int quantity)
        {
            OrderID = orderID;
            GameID = gameID;
            Quantity = quantity;
        }
    }
}