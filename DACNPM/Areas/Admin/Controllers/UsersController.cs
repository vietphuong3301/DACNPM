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
    public class UsersController : Controller
    {
        private readonly ApplicationDbcontext _context;

        public UsersController(ApplicationDbcontext context)
        {
            _context = context;
        }

        // GET: Admin/Users
        [HttpGet]
        public async Task<IActionResult> Index(string? keyword)
        {
            var users = _context.Users.Include(u => u.Role);
            if (keyword == "admin")
            {
                return View(await users.Where(u => u.RoleID == 1).ToListAsync());
            }
            return View(await users.Where(u => u.RoleID == 2).ToListAsync());
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // POST: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangeInfo(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var getuser = await _context.Users.Include(r => r.Role).SingleOrDefaultAsync(u => u.UserID == id);
            ViewData["RoleID"] = new SelectList(_context.Roles, "RoleID", "RoleName", getuser.RoleID);
            return View(getuser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeInfo(string id, User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
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
            ViewData["RoleID"] = new SelectList(_context.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
