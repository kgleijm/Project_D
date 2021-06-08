using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project_D.Models;
using Microsoft.AspNetCore.Http;

namespace Project_D.Controllers
{
    public class DataController : Controller
    {
        private readonly project_DContext _context;
        public DataController(project_DContext context)
        {
            _context = context;
        }

        // GET: Data
        public async Task<IActionResult> Index()
        {
            return View(await _context.Data.ToListAsync());
        }

        // GET: Data/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @data = await _context.Data
                .FirstOrDefaultAsync(m => m.DataID == id);
            if (@data == null)
            {
                return NotFound();
            }

            return View(@data);
        }

        // GET: Data/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Data/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataID,EnergyConsumption,GasConsumption,EnergySaving,GasSaving,Date,DepartmentID")] Data @data)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@data);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@data);
        }

        // GET: Data/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @data = await _context.Data.FindAsync(id);
            if (@data == null)
            {
                return NotFound();
            }
            return View(@data);
        }

        // POST: Data/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DataID,EnergyConsumption,GasConsumption,EnergySaving,GasSaving,Date,DepartmentID")] Data @data)
        {
            if (id != @data.DataID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataExists(@data.DataID))
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
            return View(@data);
        }

        // GET: Data/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @data = await _context.Data
                .FirstOrDefaultAsync(m => m.DataID == id);
            if (@data == null)
            {
                return NotFound();
            }

            return View(@data);
        }

        // POST: Data/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @data = await _context.Data.FindAsync(id);
            _context.Data.Remove(@data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataExists(int id)
        {
            return _context.Data.Any(e => e.DataID == id);
        }
    }
}
