using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACNPM.Data;
using DACNPM.Models;

namespace DACNPM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbcontext _context;

        public OrdersController(ApplicationDbcontext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var orders = _context.Orders.Include(o => o.User);
            ViewBag.OrderDetails = await _context.OrderDetails.ToListAsync();
            return View(await orders.ToListAsync());
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.OrderDetails = await _context.OrderDetails
                .Where(od => od.OrderID == id)
                .Include(p => p.Product)
                .ToListAsync();
            return View(order);
        }
        // GET: Admin/Orders/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, int? status)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if (status.HasValue)
            {
                order.Status = status.Value;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OrderDetails = await _context.OrderDetails
                .Where(od => od.OrderID == id)
                .Include(p => p.Product)
                .ToListAsync();
            return View(order);
        }


        // POST: Admin/Orders/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
