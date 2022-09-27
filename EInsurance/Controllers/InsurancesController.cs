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
        public async Task<ActionResult<Insurance>> GetInsurance(int id)
        {
            //if (!id.HasValue)
            //{
            //    return BadRequest();
            //}

            //try
            //{
            //    var insurance = await _context.Insurances.FindAsync(id);

            //    if (insurance == null)
            //    {
            //        return NotFound();
            //    }

            //    return Ok(insurance);
            //}
            //catch (Exception exp)
            //{
            //    return BadRequest(exp.Message);
            //}

            var insurance = await _context.Insurances.FindAsync(id);

            if (insurance == null)
            {
                return NotFound();
            }

            return insurance;
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

            _context.Entry(insurance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Insurances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Insurance>> PostInsurance(Insurance insurance)
        {
            _context.Insurances.Add(insurance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsurance", new { id = insurance.InsuranceId }, insurance);
        }

        // DELETE: api/Insurances/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Insurance>> DeleteInsurance(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }

            _context.Insurances.Remove(insurance);
            await _context.SaveChangesAsync();

            return insurance;
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurances.Any(e => e.InsuranceId == id);
        }
    }
}
