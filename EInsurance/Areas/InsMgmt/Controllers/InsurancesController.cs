using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EInsurance.Data;
using EInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EInsurance.Areas.InsMgmt.Controllers
{
    [Area("InsMgmt")]
    [Authorize]
    public class InsurancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InsurancesController> _logger;

        public InsurancesController(ApplicationDbContext context, ILogger<InsurancesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: InsMgmt/Insurances
        public async Task<IActionResult> Index()
        {
            return View(await _context.Insurances.ToListAsync());
        }

        //get method
        //public async Task<IActionResult> GetPolicyOfCategory(int filterInsuranceId)
        //{
        //    var viewmodel = await _context.InsurancePolicy
        //                                  .Where(m => m.InsuranceId == filterInsuranceId)
        //                                  .Include(m => m.Insurance)
        //                                  .ToListAsync();

        //    return View(viewName: "Index", model: viewmodel);
        //}

        // GET: InsMgmt/Insurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .FirstOrDefaultAsync(m => m.InsuranceId == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: InsMgmt/Insurances/Create

        [Authorize(Roles = "InsuranceAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsMgmt/Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "InsuranceAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsuranceId,InsuranceName")] Insurance insurance)
        {

            // Sanitize the data
            insurance.InsuranceName = insurance.InsuranceName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.Insurances.Any(c => c.InsuranceName == insurance.InsuranceName);
            if (duplicateExists)
            {
                ModelState.AddModelError("InsuranceName", "Duplicate Insurance Found!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insurance);
        }

        // GET: InsMgmt/Insurances/Edit/5

        [Authorize(Roles = "InsuranceAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: InsMgmt/Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "InsuranceAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InsuranceId,InsuranceName")] Insurance insurance)
        {
            if (id != insurance.InsuranceId)
            {
                return NotFound();
            }

            // Sanitize the data
            insurance.InsuranceName = insurance.InsuranceName.Trim();
            // Validation Checks - Server-side validation
            bool duplicateExists = _context.Insurances
                .Any(c => c.InsuranceName == insurance.InsuranceName && c.InsuranceId != insurance.InsuranceId);
            if (duplicateExists)
            {
                ModelState.AddModelError("InsuranceName", "Duplicate Insurance Found!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.InsuranceId))
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
            return View(insurance);
        }

        // GET: InsMgmt/Insurances/Delete/5

        [Authorize(Roles = "InsuranceAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .FirstOrDefaultAsync(m => m.InsuranceId == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: InsMgmt/Insurances/Delete/5
        [Authorize(Roles = "InsuranceAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            _context.Insurances.Remove(insurance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurances.Any(e => e.InsuranceId == id);
        }
    }
}
