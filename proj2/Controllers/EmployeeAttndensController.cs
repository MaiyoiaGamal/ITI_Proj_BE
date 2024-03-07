using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj2.DTO;
using proj2.Models;

namespace proj2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAttndensController : ControllerBase
    {
        private readonly HRContext _context;

        public EmployeeAttndensController(HRContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeAttndens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeAttndens>>> GetEmployeesAttndens()
        {
            var employeeAttndens = await _context.EmployeesAttndens.ToListAsync();

            
            var groupedAttndens = employeeAttndens.GroupBy(e => e.empID);

            var employeeAttndensDTOs = groupedAttndens.Select(group =>
            {
                var emp = _context.Employees.Find(group.Key);
                var listOfAttendes = group.Select(e => new ListOfAttendes
                { 
                    Date = e.Date,
                    Attendens = e.Attendens,
                    Deperture = e.Deperture
                }).ToList();

                return new EmployeeAttndensDTO
                {
                    id = group.Key,
                    name = emp.FullName,
                    ListOfAttendes = listOfAttendes
                };
            }).ToList();

            return Ok(employeeAttndensDTOs);

        }

        // GET: api/EmployeeAttndens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeAttndens>> GetEmployeeAttndens(int id )
        {
            // var employeeAttndens = await _context.EmployeesAttndens.FindAsync(id);
            var employeeAttndens = await _context.EmployeesAttndens.Where(a => a.empID == id).FirstOrDefaultAsync();


            if (employeeAttndens == null)
            {
                return NotFound();
            }

            return Ok(employeeAttndens);
        }

        [HttpGet("{id}/{date}/getbydateandid")]
        public async Task<ActionResult<EmployeeAttndens>> GetempByDateandID(int id , string date)
        {
            if (!DateOnly.TryParse(date, out DateOnly parsedStartDate))
            {
                return BadRequest("Invalid date format. Please use a valid date format.");
            }


            // var employeeAttndens = await _context.EmployeesAttndens.FindAsync(id);
            var employeeAttndens = await _context.EmployeesAttndens.Where(a => a.empID == id && a.Date == parsedStartDate).FirstOrDefaultAsync();


            if (employeeAttndens == null)
            {
                return NotFound();
            }

            return Ok(employeeAttndens);
        }






        // PUT: api/EmployeeAttndens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAttndens(int id, EmployeeAttndens employeeAttndens)
        {
            //var employee = await _context.EmployeesAttndens.FindAsync(id);
            var employee = await _context.EmployeesAttndens.Where(a => a.empID == id).FirstOrDefaultAsync();
            if (employee == null)  return BadRequest(); 
            //if (id != employeeAttndens.id)
            //{
            //    return BadRequest();
            //}

            // _context.Entry(employeeAttndens).State = EntityState.Modified;

            employee.Date = employeeAttndens.Date;
            employee.Attendens = employeeAttndens.Attendens;
            employee.Deperture = employeeAttndens.Deperture;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAttndensExists(id))
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

        //updatebyidanddate
        [HttpPut("{id}/{date}/updatebyidanddate")]
        public async Task<IActionResult> PutEmployeeAttndens2(int id, string date, EmployeeAttndens employeeAttndens)
        {
            if (!DateOnly.TryParse(date, out DateOnly parsedDate)) ;
            
                //var employee = await _context.EmployeesAttndens.FindAsync(id);
             var employee = await _context.EmployeesAttndens.Where(a => a.empID == id && a.Date == parsedDate).FirstOrDefaultAsync();

            if (employee == null) return BadRequest();
            //if (id != employeeAttndens.id)
            //{
            //    return BadRequest();
            //}

            // _context.Entry(employeeAttndens).State = EntityState.Modified;

            employee.Date = employeeAttndens.Date;
            employee.Attendens = employeeAttndens.Attendens;
            employee.Deperture = employeeAttndens.Deperture;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAttndensExists(id))
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








        // POST: api/EmployeeAttndens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeAttndens>> PostEmployeeAttndens(EmployeeAttndens emppost)
        {               
            var employee = await _context.Employees.FindAsync(emppost.empID);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var newEmployeeAttndens = new EmployeeAttndens
            {    
                Employee=employee,
                Date = emppost.Date,
                Attendens = emppost.Attendens,
                Deperture = emppost.Deperture    
            };

            _context.EmployeesAttndens.Add(newEmployeeAttndens);
            await _context.SaveChangesAsync();

            
            return CreatedAtAction("GetEmployeesAttndens", new { id = newEmployeeAttndens.id }, newEmployeeAttndens); 
        }

       



        // DELETE: api/EmployeeAttndens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAttndens(int id)
        {
            var employeeAttndens = await _context.EmployeesAttndens.FindAsync(id);
            if (employeeAttndens == null)
            {
                return NotFound();
            }

            _context.EmployeesAttndens.Remove(employeeAttndens);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeAttndensExists(int id)
        {
            return _context.EmployeesAttndens.Any(e => e.id == id);
        }
    }
}
