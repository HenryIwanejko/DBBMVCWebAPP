using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBBMVCWebApp.Models
{
    public class Basket
    {
        public List<BasketItem> BasketItems { get; private set; }

        public decimal BasketTotal { get; private set; }

        public int NumberOfItems { get; set; }
        
        public Basket(List<BasketItem> basketItems)
        {
            BasketItems = basketItems;
            BasketTotal = calculateBasketTotal();
            NumberOfItems = calculateNumberOfItems();
        }

        private decimal calculateBasketTotal() {
            decimal total = 0;
            foreach(var basketItem in BasketItems) {
                total += basketItem.Price * basketItem.PurchaseQuantity;
            }
            return total;
        }

        private int calculateNumberOfItems() {
            int numberOfItems = 0;
            foreach(var basketItem in BasketItems) {
                numberOfItems += basketItem.PurchaseQuantity;
            }
            return numberOfItems;
        }
    }
    
}