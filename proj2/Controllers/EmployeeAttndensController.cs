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
        //public async Task<ActionResult<IEnumerable<EmployeeAttndens>>> GetEmployeesAttndens()
        //{
        //    var employeeAttndens = await _context.EmployeesAttndens.ToListAsync();

        //    var lattime = new TimeOnly(18, 0, 0);
        //    var plustime = new TimeOnly(9,0, 0);
        //    var groupedAttndens = employeeAttndens.GroupBy(e => e.empID);

        //    var employeeAttndensDTOs = groupedAttndens.Select(group =>
        //    {
        //        var emp = _context.Employees.Find(group.Key);

        //        var listOfAttendes = group.Select(e => new ListOfAttendes
        //        {
        //            Date = e.Date,
        //            Attendens = e.Attendens,
        //            Deperture = e.Deperture,
        //            plus = (e.Deperture - lattime).Hours,
        //            //late = (e.Attendens - plustime).Hours ? e.Attendens > lattime : 
        //            late = e.Attendens > plustime || e.Deperture < lattime ? 
        //            (e.Attendens - plustime).Hours + (lattime - e.Deperture).Hours : (e.Attendens - plustime).Hours

        //        }).ToList();

        //        return new EmployeeAttndensDTO
        //        {
        //            id = group.Key,
        //            name = emp.FullName,
        //            ListOfAttendes = listOfAttendes

        //        };
        //    }).ToList();

        //    return Ok(employeeAttndensDTOs);

        //}
        public async Task<ActionResult<IEnumerable<EmployeeAttndens>>> GetEmployeesAttndens()
        {
            var employeeAttndens = await _context.EmployeesAttndens.ToListAsync();

            var lattime = new TimeOnly(18, 0, 0);
            var plustime = new TimeOnly(9, 0, 0);
            var groupedAttndens = employeeAttndens.GroupBy(e => e.empID);

            var employeeAttndensDTOs = groupedAttndens.Select(group =>
            {
                var emp = _context.Employees.Find(group.Key);

                var listOfAttendes = group.Select(e =>
                {
                    int plusHours = 0;
                    int lateHours = 0;

                    // Calculate plus hours if departure is after the plus time
                    if (e.Deperture > lattime)
                    {
                        plusHours = (e.Deperture - lattime).Hours;
                    }
                    else
                    {
                        lateHours =  (lattime - e.Deperture).Hours;
                    }

                    if (e.Attendens > plustime)
                    {
                        lateHours += (lattime - e.Deperture).Hours;
                    }


                    return new ListOfAttendes
                    {
                        Date = e.Date,
                        Attendens = e.Attendens,
                        Deperture = e.Deperture,
                        plus = plusHours,
                        late = lateHours
                    };
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
        public async Task<ActionResult<EmployeeAttndens>> GetEmployeeAttndens(int id)
        {

            var employeeAttndens = await _context.EmployeesAttndens.Where(a => a.empID == id).FirstOrDefaultAsync();


            if (employeeAttndens == null)
            {
                return NotFound();
            }

            return Ok(employeeAttndens);
        }

        [HttpGet("{id}/{date}/getbydateandid")]
        public async Task<ActionResult<EmployeeAttndens>> GetempByDateandID(int id, string date)
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
            var employee = await _context.EmployeesAttndens.Where(a => a.empID == id).FirstOrDefaultAsync();
            if (employee == null) return BadRequest();
            

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
            var todayDate = DateOnly.FromDateTime(DateTime.UtcNow);

            if (employeeAttndens.Date > todayDate)
            {
                return BadRequest("can't select date after today date");
            }

            if (employeeAttndens.Date.Year < 2020)
            {
                return BadRequest("Can't select date before 2020");
            }

            if (employeeAttndens.Attendens < new TimeOnly(9, 00, 00))
            {
                return BadRequest("Can't select time before 9:00:00");
            }
            if (employeeAttndens.Attendens > new TimeOnly(12, 00, 00))
            {
                return BadRequest("Can't select time after 12:00:00");
            }


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
            var holidayday1 = _context.Settings.Select(s => s.HolidayDayOne).FirstOrDefault();
            var holidayday2 = _context.Settings.Select(s => s.HolidayDayTwo).FirstOrDefault();
            var deperturetime = new TimeOnly(19, 0, 0);
            var todayDate = DateOnly.FromDateTime(DateTime.UtcNow);

            if (emppost.Date.DayOfWeek == holidayday1 || emppost.Date.DayOfWeek == holidayday2)
            {
                return BadRequest("Attendance cannot be posted on weekends.");
            }



            if (IsHoliday(emppost.Date))
            {
                return BadRequest("Attendance cannot be posted on holidays.");
            }

            var employee = await _context.Employees.FindAsync(emppost.empID);

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }
            var existingAttendance = await _context.EmployeesAttndens
             .FirstOrDefaultAsync(e => e.empID == emppost.empID && e.Date == emppost.Date);

            if (existingAttendance != null)
            {
                return BadRequest("Attendance for the same date already exists.");
            }

            if (emppost.Date > todayDate)
            {
                return BadRequest("can't select date after today date");
            }

            if (emppost.Date.Year < 2020)
            {
                return BadRequest("Can't select date before 2020");
            }

            if (emppost.Attendens < new TimeOnly(9, 00, 00))
            {
                return BadRequest("Can't select time before 9:00:00");
            } 
            if (emppost.Attendens > new TimeOnly(12, 00, 00))
            {
                return BadRequest("Can't select time  after 12:00:00");
            }



            var newEmployeeAttndens = new EmployeeAttndens
            {
                Employee = employee,
                Date = emppost.Date,
                Attendens = emppost.Attendens,
                Deperture = emppost.Deperture,
            };



            _context.EmployeesAttndens.Add(newEmployeeAttndens);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetEmployeesAttndens", new { id = newEmployeeAttndens.id }, newEmployeeAttndens);
        }

        //fn of date
        private bool IsHoliday(DateOnly date)
        {
            var holidays = _context.Holidays.ToList();

            foreach (var holiday in holidays)
            {
                if (date.Day == holiday.Date.Day)
                {
                    return true;
                }
            }
            return false;
        }

        // DELETE: api/EmployeeAttndens/5
        [HttpDelete("{employeeId}/attendance/{date}")]
        public async Task<IActionResult> DeleteEmployeeAttendance(int employeeId, string date)
        {
            DateOnly.TryParse(date,out DateOnly parsed);
            var employeeAttendance = await _context.EmployeesAttndens
            .FirstOrDefaultAsync(e => e.empID == employeeId && e.Date == parsed);

            if (employeeAttendance == null)
            {
                return NotFound();
            }

            _context.EmployeesAttndens.Remove(employeeAttendance);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EmployeeAttndensExists(int id)
        {
            return _context.EmployeesAttndens.Any(e => e.id == id);
        }
    }
}
