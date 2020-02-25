using System;
using System.Collections.Generic;

namespace DBBMVCWebApp.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public string UserId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal OrderTotal { get; set; }

        public Order() {}
        
        public Order(string userId, DateTime orderDate, decimal orderTotal)
        {
            UserId = userId;
            OrderDate = orderDate;
            OrderTotal = orderTotal;
        }
    }
}