using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EInsurance.Data;
using EInsurance.Models;
using Microsoft.Extensions.Logging;

namespace EInsurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InsurancesController> _logger;

        public InsurancesController(ApplicationDbContext context, ILogger<InsurancesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Insurances
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Insurance>>> GetInsurances()
        public async Task<IActionResult> GetInsurances()
        {
            try
            {
                var insurances = await _context.Insurances
                .Include(c => c.InsurancePolicy)
                .ToListAsync();

                // Check if data exists in the Database
                if (insurances == null)
                {
                    return NotFound();          // RETURN: No data was found            HTTP 404
                }
                return Ok(insurances);          // RETURN: OkObjectResult - good result HTTP 200
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message); // RETURN: BadResult                    HTTP 400
            }
        }

        // GET: api/Insurances/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<Insurance>> GetInsurance(int? id)
        public async Task<IActionResult> GetInsurance(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var insurance = await _context.Insurances.FindAsync(id);

                if (insurance == null)
                {
                    return NotFound();
                }

                return Ok(insurance);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }

            //var insurance = await _context.Insurances.FindAsync(id);

            //if (insurance == null)
            //{
            //    return NotFound();
            //}

            //return insurance;
        }

        // PUT: api/Insurances/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsurance(int id, Insurance insurance)
        {
            if (id != insurance.InsuranceId)
            {
                return BadRequest();
            }

            // Sanitize the Data
            insurance.InsuranceName = insurance.InsuranceName.Trim();

            // Server Side Validation
            bool isDuplicateFound = _context.Insurances.Any(c => c.InsuranceName == insurance.InsuranceName);
            if (isDuplicateFound)
            {
                ModelState.AddModelError("POST", "Duplicate Insurance Found!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(insurance).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException exp)
                {
                    if (!InsuranceExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("PUT", exp.Message);
                    }
                }
            }

            return BadRequest(ModelState);

            //if (id != insurance.InsuranceId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(insurance).State = EntityState.Modified;


            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!InsuranceExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }

        // POST: api/Insurances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //public async Task<ActionResult<Insurance>> PostInsurance(Insurance insurance)
        public async Task<IActionResult> PostInsurance(Insurance insurance)
        {
            // Sanitize the Data
            insurance.InsuranceName = insurance.InsuranceName.Trim();

            // Server Side Validation
            bool isDuplicateFound = _context.Insurances.Any(c => c.InsuranceName == insurance.InsuranceName);
            if (isDuplicateFound)
            {
                ModelState.AddModelError("POST", "Duplicate Category Found!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Insurances.Add(insurance);
                    await _context.SaveChangesAsync();

                    // return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);

                    // To enforce that the HTTP RESPONSE CODE 201 "CREATED", package the response.
                    var result = CreatedAtAction("GetInsurance", new { id = insurance.InsuranceId }, insurance);
                    return Ok(result);
                }
                catch (System.Exception exp)
                {
                    ModelState.AddModelError("POST", exp.Message);
                }
            }

            return BadRequest(ModelState);

            //_context.Insurances.Add(insurance);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetInsurance", new { id = insurance.InsuranceId }, insurance);
        }

        // DELETE: api/Insurances/5
        [HttpDelete("{id}")]
        //public async Task<ActionResult<Insurance>> DeleteInsurance(int id)
        public async Task<IActionResult> DeleteInsurance(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var insurance = await _context.Insurances.FindAsync(id);
                if (insurance == null)
                {
                    return NotFound();
                }

                _context.Insurances.Remove(insurance);
                await _context.SaveChangesAsync();

                return Ok(insurance);
            }
            catch (System.Exception exp)
            {
                ModelState.AddModelError("DELETE", exp.Message);
                return BadRequest(ModelState);
            }

            //var insurance = await _context.Insurances.FindAsync(id);
            //if (insurance == null)
            //{
            //    return NotFound();
            //}

            //_context.Insurances.Remove(insurance);
            //await _context.SaveChangesAsync();

            //return insurance;
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurances.Any(e => e.InsuranceId == id);
        }
    }
}
