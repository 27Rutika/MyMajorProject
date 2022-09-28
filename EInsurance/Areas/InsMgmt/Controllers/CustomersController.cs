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

namespace EInsurance.Areas.InsMgmt.Controllers
{
    [Area("InsMgmt")]
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InsMgmt/Customers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Customer.Include(c => c.InsurancePolicy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InsMgmt/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.InsurancePolicy)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: InsMgmt/Customers/Create
       
        public IActionResult Create(int id)
        {
            //ViewData["PolicyId"] = new SelectList(_context.Set<InsurancePolicy>(), "PolicyId", "PolicyName");
            //return View();
            var policy = _context.InsurancePolicy.SingleOrDefault(c => c.PolicyId == id);
            ViewBag.PolicyId = policy.PolicyId;
            ViewBag.PolicyName = policy.PolicyName;
            //ViewData["PlanId"] = new SelectList(_context.Plan, "PlanId", "PlanType");
            return View();
        }

        // POST: InsMgmt/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FullName,Age,Email,PhoneNumber,BirthDate,Gender,Address,Country,PolicyId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PolicyId"] = new SelectList(_context.Set<InsurancePolicy>(), "PolicyId", "PolicyName", customer.PolicyId);
            return View(customer);
        }

        // GET: InsMgmt/Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["PolicyId"] = new SelectList(_context.Set<InsurancePolicy>(), "PolicyId", "PolicyName", customer.PolicyId);
            return View(customer);
        }

        // POST: InsMgmt/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FullName,Age,Email,PhoneNumber,BirthDate,Gender,Address,Country,PolicyId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["PolicyId"] = new SelectList(_context.Set<InsurancePolicy>(), "PolicyId", "PolicyName", customer.PolicyId);
            return View(customer);
        }

        // GET: InsMgmt/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.InsurancePolicy)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: InsMgmt/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
