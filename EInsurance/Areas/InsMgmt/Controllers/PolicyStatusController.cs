using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EInsurance.Data;
using EInsurance.Models;

namespace EInsurance.Areas.InsMgmt.Controllers
{
    [Area("InsMgmt")]
    public class PolicyStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PolicyStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InsMgmt/PolicyStatus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PolicyStatus.Include(p => p.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InsMgmt/PolicyStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyStatus = await _context.PolicyStatus
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.ApprovedPolicyId == id);
            if (policyStatus == null)
            {
                return NotFound();
            }

            return View(policyStatus);
        }

        // GET: InsMgmt/PolicyStatus/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName");
            return View();
        }

        // POST: InsMgmt/PolicyStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApprovedPolicyId,Status,ApporovalDate,CustomerId")] PolicyStatus policyStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(policyStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", policyStatus.CustomerId);
            return View(policyStatus);
        }

        // GET: InsMgmt/PolicyStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyStatus = await _context.PolicyStatus.FindAsync(id);
            if (policyStatus == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", policyStatus.CustomerId);
            return View(policyStatus);
        }

        // POST: InsMgmt/PolicyStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApprovedPolicyId,Status,ApporovalDate,CustomerId")] PolicyStatus policyStatus)
        {
            if (id != policyStatus.ApprovedPolicyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policyStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyStatusExists(policyStatus.ApprovedPolicyId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", policyStatus.CustomerId);
            return View(policyStatus);
        }

        // GET: InsMgmt/PolicyStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyStatus = await _context.PolicyStatus
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.ApprovedPolicyId == id);
            if (policyStatus == null)
            {
                return NotFound();
            }

            return View(policyStatus);
        }

        // POST: InsMgmt/PolicyStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policyStatus = await _context.PolicyStatus.FindAsync(id);
            _context.PolicyStatus.Remove(policyStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PolicyStatusExists(int id)
        {
            return _context.PolicyStatus.Any(e => e.ApprovedPolicyId == id);
        }
    }
}
