using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACNPM.Data;
using DACNPM.Models;
using DACNPM.Utils;

namespace DACNPM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbcontext _context;

        public ProductTypesController(ApplicationDbcontext context)
        {
            _context = context;
        }

        // GET: Admin/ProductTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductType.ToListAsync());
        }

        // GET: Admin/ProductTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ProductType == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType
                .FirstOrDefaultAsync(m => m.TypeID == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // GET: Admin/ProductTypes/Create
        public IActionResult Create()
        {
            var ID = "TYP0001";
            var IDnum = _context.ProductType.Count();
            if (IDnum > 0)
            {
                ID = _context.ProductType.OrderByDescending(s => s.TypeID).First().TypeID;
                ViewData["TypeID"] = GenerateID.AutoGenerateCode(ID);
            }
            else
            {
                ViewData["TypeID"] = ID;
            }
            return View();
        }

        // POST: Admin/ProductTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeID,Type")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                productType.Type = productType.Type.ToUpper();
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        // GET: Admin/ProductTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ProductType == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        // POST: Admin/ProductTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TypeID,Type")] ProductType productType)
        {
            if (id != productType.TypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productType.Type = productType.Type.ToUpper();
                    _context.Update(productType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.TypeID))
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
            return View(productType);
        }


        // POST: Admin/ProductTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.ProductType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductType'  is null.");
            }
            var productType = await _context.ProductType.FindAsync(id);
            if (productType != null)
            {
                _context.ProductType.Remove(productType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTypeExists(string id)
        {
            return _context.ProductType.Any(e => e.TypeID == id);
        }
    }
}
