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
    public class InsurancePoliciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsurancePoliciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InsMgmt/InsurancePolicies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InsurancePolicy.Include(i => i.Insurance);
            return View(await applicationDbContext.ToListAsync());
        }

        //get insurance category
        public async Task<IActionResult> GetPolicyOfCategory(int filterInsuranceId)
        {
            var viewmodel = await _context.InsurancePolicy
                                          .Where(m => m.InsuranceId == filterInsuranceId)
                                          .Include(m => m.Insurance)
                                          .ToListAsync();

            return View(viewName: "Index", model: viewmodel);
        }

        // GET: InsMgmt/InsurancePolicies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurancePolicy = await _context.InsurancePolicy
                .Include(i => i.Insurance)
                .FirstOrDefaultAsync(m => m.PolicyId == id);
            if (insurancePolicy == null)
            {
                return NotFound();
            }

            return View(insurancePolicy);
        }

        // GET: InsMgmt/InsurancePolicies/Create
        public IActionResult Create()
        {
            ViewData["InsuranceId"] = new SelectList(_context.Insurances, "InsuranceId", "InsuranceName");
            return View();
        }

        // POST: InsMgmt/InsurancePolicies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PolicyId,PolicyName,SumAssurance,Premium,Tenure,CreatedOn,InsuranceId")] InsurancePolicy insurancePolicy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurancePolicy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InsuranceId"] = new SelectList(_context.Insurances, "InsuranceId", "InsuranceName", insurancePolicy.InsuranceId);
            return View(insurancePolicy);
        }

        // GET: InsMgmt/InsurancePolicies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurancePolicy = await _context.InsurancePolicy.FindAsync(id);
            if (insurancePolicy == null)
            {
                return NotFound();
            }
            ViewData["InsuranceId"] = new SelectList(_context.Insurances, "InsuranceId", "InsuranceName", insurancePolicy.InsuranceId);
            return View(insurancePolicy);
        }

        // POST: InsMgmt/InsurancePolicies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PolicyId,PolicyName,SumAssurance,Premium,Tenure,CreatedOn,InsuranceId")] InsurancePolicy insurancePolicy)
        {
            if (id != insurancePolicy.PolicyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurancePolicy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsurancePolicyExists(insurancePolicy.PolicyId))
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
            ViewData["InsuranceId"] = new SelectList(_context.Insurances, "InsuranceId", "InsuranceName", insurancePolicy.InsuranceId);
            return View(insurancePolicy);
        }

        // GET: InsMgmt/InsurancePolicies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurancePolicy = await _context.InsurancePolicy
                .Include(i => i.Insurance)
                .FirstOrDefaultAsync(m => m.PolicyId == id);
            if (insurancePolicy == null)
            {
                return NotFound();
            }

            return View(insurancePolicy);
        }

        // POST: InsMgmt/InsurancePolicies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insurancePolicy = await _context.InsurancePolicy.FindAsync(id);
            _context.InsurancePolicy.Remove(insurancePolicy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsurancePolicyExists(int id)
        {
            return _context.InsurancePolicy.Any(e => e.PolicyId == id);
        }
    }
}
