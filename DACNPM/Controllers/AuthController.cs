using DACNPM.Data;
using DACNPM.Models;
using DACNPM.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACNPM.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public AuthController(ApplicationDbcontext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            var ID = "USER0001";
            var IDnum = _context.Users.Count();
            if (IDnum > 0)
            {
                ID = _context.Users.OrderByDescending(s => s.UserID).First().UserID;
                ViewData["UserID"] = GenerateID.AutoGenerateCode(ID);
            }
            else
            {
                ViewData["UserID"] = ID;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email, Password")] User user)
        {
            if (user.Email != null && user.Password != null)
            {
                var data = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
                if(data != null)
                {
                    HttpContext.Session.SetString("UserID", data.UserID);
                    HttpContext.Session.SetString("FirstName", data.FirstName);
                    HttpContext.Session.SetString("LastName", data.LastName);
                    HttpContext.Session.SetString("Email", data.Email);
                    HttpContext.Session.SetInt32("RoleID", data.RoleID);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Email or password is incorrect!";
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("UserID,FirstName, LastName, Email, Password, Phone, FullAddress, RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                var data = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);
                if (data != null)
                {
                    TempData["ErrorMessage"] = "This Email is already used for registration!";
                }
                else
                {
                    _context.Users.Add(new User
                    {
                        UserID = user.UserID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,   
                        Phone = user.Phone,
                        FullAddress = user.FullAddress,
                        RoleID = user.RoleID,
                    });
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));

                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
