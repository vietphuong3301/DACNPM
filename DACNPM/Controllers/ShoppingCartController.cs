using DACNPM.Data;
using DACNPM.Models;
using DACNPM.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DACNPM.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public ShoppingCartController(ApplicationDbcontext context)
        {
            _context = context;
        }

        public List<CartItem> Cart
        {
            get
            {
                var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
                if(cart == default(List<CartItem>))
                {
                    cart = new List<CartItem>();
                }
                return cart;
            }
        }
        public IActionResult Index()
        {
            return View(Cart);
        }

        [HttpPost, HttpGet]
        public  IActionResult AddtoCart(string id, int? amount)
        {
            List<CartItem> cart = Cart;

            CartItem item = cart.SingleOrDefault(p => p.Product.ProductID == id)!;
            if(item != null)
            {
                foreach (var i in cart)
                {
                    if (i.Product.ProductID == item.Product.ProductID)
                    {
                        if (amount.HasValue)
                        {

                            i.Amount += amount.Value;
                        }
                        else
                        {
                            i.Amount++;
                        }
                    }
                }
            }
            else
            {
                var product = _context.Products.SingleOrDefault(p => p.ProductID == id);
                item = new CartItem
                {
                    Amount = amount.HasValue ? amount.Value : 1,
                    Product = product!
                };
                cart.Add(item);
            }

            HttpContext.Session.Set<List<CartItem>>("Cart", cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateItem(string[] products, int[] amounts)
        {
            List<CartItem> cart = Cart;
            for(int i = 0; i < cart.Count; i++)
            {
                var id = cart.SingleOrDefault(p => p.Product.ProductID == products[i])!.Product.ProductID;
                if(id != null && products[i] == id)
                {
                    cart.ElementAt(i).Amount = amounts[i];
                }
            }
            HttpContext.Session.Set<List<CartItem>>("Cart", cart);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RemoveItem(string id)
        {
            List<CartItem> cart = Cart;

            CartItem item = cart.SingleOrDefault(p => p.Product.ProductID == id)!;
            if (item != null)
            {
                cart.Remove(item);
            }

            HttpContext.Session.Set<List<CartItem>>("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveAll()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var isUser = HttpContext.Session.GetString("UserID");
            List<CartItem> cart = Cart;
            if(isUser == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userInfo = await _context.Users.FindAsync(isUser);
            ViewBag.UserInfo = userInfo;
            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string id,string firstname, string lastname, string email, string phone, string address, string? note)
        {
            List<CartItem> cart = Cart;
            Order order = new Order();
            order.FullName = firstname +" "+ lastname;
            order.Address = address;
            order.Email = email;
            order.Note = note;
            order.UserID = id;
            order.Phone = phone;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.OrderID = order.OrderID;
                orderDetails.ProductID = item.Product.ProductID;
                orderDetails.Quantity = item.Amount;
                orderDetails.SubPrice = item.totalPrice;
                _context.OrderDetails.Add(orderDetails);
            }
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("OrderSuccess");
        }

        public async Task<IActionResult> OrderSuccess()
        {
            var userid = HttpContext.Session.GetString("UserID");
            if(userid == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var order = await _context.Orders.OrderByDescending(o => o.CreatedAt).FirstOrDefaultAsync(p => p.UserID == userid);
            var orderdetails = await _context.OrderDetails.Where(od => od.OrderID == order.OrderID).Include(od => od.Product).ToListAsync();
            ViewBag.OrderDetails = orderdetails;
            return View(order);
        }
        


    }
}
