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

namespace DACNPM.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbcontext _context;

        public CustomersController(ApplicationDbcontext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            
            var session = HttpContext.Session.GetString("UserID");
            if(session == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == session);
            return View(user);
        }
        public async Task<IActionResult> Update()
        {
            var session = HttpContext.Session.GetString("UserID");
            if (session == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users.FindAsync(session);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("UserID,FirstName,LastName,Email,Password,Phone,FullAddress, RoleID")] User user)
        {
            var session = HttpContext.Session.GetString("UserID");
            if (session == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (user != null)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var session = HttpContext.Session.GetString("UserID");
         
            if(session == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var orders = await _context.Orders.Where(o => o.UserID == session).ToListAsync();
            ViewBag.OrderDetails = await _context.OrderDetails.Include(od => od.Product).ToListAsync();
            
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var orderdetail = await _context.Orders.FindAsync(id);
            ViewBag.OrderDetails = await _context.OrderDetails
                .Where(od => od.OrderID == orderdetail.OrderID)
                .Include(od => od.Product)
                .ToListAsync();
            return View(orderdetail);
        }
        private bool UserExists(string id)
        {
          return _context.Users.Any(e => e.UserID == id);
        }
    }
}
