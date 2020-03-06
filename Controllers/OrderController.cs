using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DBBMVCWebApp.Data;
using DBBMVCWebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DBBMVCWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Basket() 
        {   
            if (User.Identity.IsAuthenticated) 
            {
                ViewData["Message"] = "Basket";     
                if (HttpContext.Session.GetString("basket") != null) {
                    List<BasketItem> basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(HttpContext.Session.GetString("basket"));
                    Basket basket = new Basket(basketItems);
                    return View(basket);
                }
                return View();
            }
            return Redirect("/Account/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Basket(int? id, int? purchaseQuantity) 
        {   
            if (User.Identity.IsAuthenticated) 
            {   

                if (id == null || purchaseQuantity == null) {
                    return BadRequest();
                }

                if (HttpContext.Session.GetString("basket") == null) {
                    HttpContext.Session.SetString("basket", string.Empty); 
                }

                string value = HttpContext.Session.GetString("basket");
                List<BasketItem> basket = JsonConvert.DeserializeObject<List<BasketItem>>(value) ?? new List<BasketItem>();

                Game game = _context.Games.Where(i => i.GameID == id).FirstOrDefault();

                BasketItem basketItem;

                if (getBasketItemFromId(basket, id) == null) {
                    basketItem = new BasketItem(game.GameID,
                        game.OwnerID,
                        purchaseQuantity.GetValueOrDefault(),
                        game.Name,
                        game.Description,
                        game.Price,
                        game.GameImage
                     );
                     basket.Add(basketItem);
                } else {
                    basketItem = getBasketItemFromId(basket, id);
                    if (game.Quantity >= basketItem.PurchaseQuantity + purchaseQuantity.GetValueOrDefault()) {
                        basketItem.PurchaseQuantity += purchaseQuantity.GetValueOrDefault();
                    }
                }

                HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basket));

                return Redirect("/Order/Basket");
            }
            return Redirect("/Account/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromBasket(int? id) 
        {   
            if (User.Identity.IsAuthenticated) 
            {
                if (HttpContext.Session.GetString("basket") == null) {
                   return Redirect("/Order/Basket");
                }

                string value = HttpContext.Session.GetString("basket");
                List<BasketItem> basket = JsonConvert.DeserializeObject<List<BasketItem>>(value) ?? new List<BasketItem>();
                
                BasketItem gameToRemove = getBasketItemFromId(basket, id);

                basket.Remove(gameToRemove);

                HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basket));

                return Redirect("/Order/Basket");
            }
            return Redirect("/Account/Login");
        }
        

        private BasketItem getBasketItemFromId(List<BasketItem> games, int? id) {
            foreach (var game in games) {
                if (game.GameID == id) {
                    return game;
                }
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout() {
            if (User.Identity.IsAuthenticated) 
            {
                if (HttpContext.Session.GetString("basket") == null) {
                   return Redirect("/Order/Basket");
                }

                string value = HttpContext.Session.GetString("basket");
                List<BasketItem> basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(value) ?? new List<BasketItem>();
                Basket basket = new Basket(basketItems);

                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);    
                Order order = new Order(userId, DateTime.Now, basket.BasketTotal);

                _context.Orders.Add(order);
                _context.SaveChanges();

                List<OrderItem> orderItems = new List<OrderItem>();

                foreach(var basketItem in basket.BasketItems) {
                    OrderItem orderItem = new OrderItem(
                        order.OrderID,
                        basketItem.GameID,
                        basketItem.PurchaseQuantity
                    );

                    orderItems.Add(orderItem);

                    Game game = _context.Games.Where(i => i.GameID == orderItem.GameID).FirstOrDefault();
                    
                    game.Quantity -= orderItem.Quantity;
                }

                order.OrderItems = orderItems;

                _context.SaveChanges();

                return Redirect("/Order/OrderConfirmation/" + order.OrderID);
            }
            return Redirect("/Account/Login");
        }

        public IActionResult OrderConfirmation(int? id) 
        {   
            if (User.Identity.IsAuthenticated) 
            {
                Order order = _context.Orders.Where(o => o.OrderID == id).FirstOrDefault();

                if (order.UserId != this.User.FindFirstValue(ClaimTypes.NameIdentifier)) {
                    return Unauthorized();
                }

                List<OrderItem> orderItems = _context.OrderItems.Where(item => item.OrderID == order.OrderID).ToList();
                
                List<Game> games = new List<Game>();

                foreach (var orderItem in orderItems) {
                    Game game = _context.Games.Where(item => item.GameID == orderItem.GameID).FirstOrDefault();
                    games.Add(game);
                }
                   
                OrderConfirmation orderConfirmation = new OrderConfirmation(order, games);

                ViewData["Message"] = "Order Successful";
                return View(orderConfirmation);
            }
            return Redirect("/Account/Login");
        }

    }
}