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
    }
}