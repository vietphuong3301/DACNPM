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
        public async Task<IActionResult> Orders(int? page)
        {
            var session = HttpContext.Session.GetString("UserID");
         
            if(session == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var orders = await _context.Orders.Where(o => o.UserID == session).ToListAsync();

            int pageSize = 5;

            int pageNumber = (page ?? 1);

            var pagedOrders = await orders.ToPagedListAsync(pageNumber, pageSize);

            ViewBag.OrderDetails = await _context.OrderDetails.Include(od => od.Product).ToListAsync();
            
            return View(pagedOrders);
        }
        [HttpGet]
        public async Task<IActionResult> CancelOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = 2; // Assuming '2' is the status code for 'Cancelled'
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Orders));
        }
        [HttpGet]
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = 3; // Assuming '3' is the status code for 'Complete'
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Orders));
        }
        [HttpGet]
        public async Task<IActionResult> Delivery(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = 1; // Assuming '1' is the status code for 'Delivery'
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Orders));
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
