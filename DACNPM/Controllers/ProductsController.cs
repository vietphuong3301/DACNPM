using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACNPM.Data;
using DACNPM.Models;
using X.PagedList;
using DACNPM.Utils;

namespace DACNPM.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbcontext _context;

        public ProductsController(ApplicationDbcontext context)
        {
            _context = context;
        }

        public List<Product> WishList
        {
            get
            {
                var wishlist = HttpContext.Session.Get<List<Product>>("WishList");
                if(wishlist == default(List<Product>))
                {
                    wishlist = new List<Product>();
                }
                return wishlist;
            }
        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> Index(string? id,int? page, string? keyword, string? sortBy, double? minPrice, double? maxPrice)
        {
            int currentPage = page ?? 1;
            int pageSize = 9;
            var products = from p in _context.Products select p;

            ViewData["Keyword"] = keyword;
            ViewData["SortBy"] = sortBy;
            ViewData["Categories"] = await _context.Categories.ToListAsync();
            ViewData["Types"] = await _context.ProductType.ToListAsync();

            //get products by catid or type
            if (!string.IsNullOrEmpty(id))
            {
                products = products.Where(p => p.CategoryID == id || p.TypeID == id)
                    .Include(c => c.Category)
                    .Include(t => t.ProductType);
            }

            //get products by search keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.ProductName.Contains(keyword))
                    .Include(c => c.Category)
                    .Include(t => t.ProductType);
                if(products.Count() == 0)
                {
                    return View("ProductNotFound");
                }
            }

            //get products by sort
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "price_asc":
                        products = products.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    case "name_desc":
                        products = products.OrderByDescending(p => p.ProductName);
                        break;
                    default:
                        products = products.OrderBy(p => p.ProductName);
                        break;
                }
            }
            // get products by price range
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                double min = Convert.ToDouble(minPrice.Value);
                double max = Convert.ToDouble(maxPrice.Value);
                products = products.Where(p => p.Price >= min && p.Price <= max)
                    .Include(c => c.Category)
                    .Include(t => t.ProductType);
            }

            return View(products.ToPagedList(currentPage, pageSize));
        }

        // GET: Products/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id, string? idParam)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category).Include(t => t.ProductType)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            var relatedProds = from p in _context.Products select p;
            if (!string.IsNullOrEmpty(idParam))
            {
                relatedProds = relatedProds.Where(p => p.CategoryID == idParam && p.ProductID != id);
                    
            }

            ViewData["RelatedProducts"] = relatedProds.Include(p => p.ProductType).Include(p => p.Category).ToList();
            return View(product);
        }

        // GET: Products/AddtoWishlist

        [HttpGet]
        public IActionResult AddtoWishlist(string? id)
        {
            List<Product> wishlist = WishList;
            if(id != null)
            {
                var check = wishlist.SingleOrDefault(p => p.ProductID == id);
                if(check == null)
                {
                    var product = _context.Products
                        .Include(c => c.Category)
                        .Include(t => t.ProductType)
                        .SingleOrDefault(p => p.ProductID == id);
                    wishlist.Add(product);
                 
                }
                else
                {
                    wishlist.Remove(check);
                    
                }
            }
            HttpContext.Session.Set<List<Product>>("WishList", wishlist);
            return RedirectToAction("Wishlist");
        }

        //// GET: Products/Wishlist
        [HttpGet]
        public IActionResult Wishlist(int? page)
        {
            List<Product> wishlist = WishList;
            int currentPage = page ?? 1;
            int pageSize = 8;
            return View(wishlist.ToPagedList(currentPage, pageSize));
        }


    }
}
