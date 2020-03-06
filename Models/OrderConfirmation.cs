using System;
using System.Collections.Generic;

namespace DBBMVCWebApp.Models
{
    public class OrderConfirmation
    {
        public Order Order { get; set; }

        public List<Game> OrderItems { get; set; }
        
        public OrderConfirmation(Order order, List<Game> orderItems)
        {
            Order = order;
            OrderItems = orderItems;
        }
    }
}