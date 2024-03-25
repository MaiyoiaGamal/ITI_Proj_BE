using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj2.Models;
using proj2.DTO;
using System.Runtime.Intrinsics.X86;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System;

namespace proj2.Controllers
{
    [EnableCors("MyCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly HRContext _context;

        public EmployeesController(HRContext context)
        {
            _context = context;
        }

        // GET: api/EmployeesTest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var empList = await _context.Employees.Where(p => p.IsDeleted == false).Include(p=> p.EmployeeAttndens).ToListAsync();
            List<EmployeeDTO> employeeDTOs = new List<EmployeeDTO>();

            foreach (var emp in empList)
            {
                EmployeeDTO empDTO = new EmployeeDTO()
                {
                    Id = emp.Id,
                    FullName = emp.FullName,
                    SSN = emp.SSN,
                    Address = emp.Address,
                    PhoneNumber = emp.PhoneNumber,
                    Nationality = emp.Nationality,
                    Sex = emp.Sex,
                    Email = emp.Email,
                    BirthDate = emp.BirthDate,
                    ContractDate = emp.ContractDate,
                    Salary = emp.Salary,
                    IsDeleted = emp.IsDeleted,
                    //Attendens = emp.EmployeeAttndens?.SelectMany(p => new List<DateTime?> { p.Attendens }).ToList(),
                    //Deperture = emp.EmployeeAttndens?.SelectMany(p => new List<DateTime?> { p.Deperture }).ToList(),
                    //Date = emp.EmployeeAttndens?.Select(p => p.Date).FirstOrDefault(),
                    //Attendens = emp.EmployeeAttndens?.Select(p => p.Attendens).FirstOrDefault(),
                    //Deperture = emp.EmployeeAttndens?.Select(p => p.Deperture).FirstOrDefault(),
                    
            };
                EmployeeAttndensDTO empAttndensDTO = new EmployeeAttndensDTO
                {
                    id = emp.Id,
                    name = emp.FullName,
                    ListOfAttendes = emp.EmployeeAttndens?.Select(e => new ListOfAttendes
                    {
                        Date = e.Date,
                        Attendens = e.Attendens,
                        Deperture = e.Deperture,
                        plus = e.plus,
                        late = e.late,
                        
                    }).ToList()
                };            
                empDTO.EmployeeAttndens = empAttndensDTO;
                employeeDTOs.Add(empDTO);
            }
            return Ok(employeeDTOs);
        }

        // GET: api/EmployeesTest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/EmployeesTest/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (id != employee.Id && existingEmployee == null)
            {
                return BadRequest();
            }
            //_context.Entry(employee).State = EntityState.Modified;
            existingEmployee.FullName = employee.FullName;
            existingEmployee.Address = employee.Address;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.BirthDate = employee.BirthDate;
            existingEmployee.Email = employee.Email;
            existingEmployee.ContractDate = employee.ContractDate;
            existingEmployee.IsDeleted = employee.IsDeleted;
            existingEmployee.SSN = employee.SSN;
            existingEmployee.Sex = employee.Sex;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Nationality = employee.Nationality;
                
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/EmployeesTest
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            if (await _context.Employees.AnyAsync(e => e.FullName == employee.FullName))
            {
                return Conflict("Employee with the same name already exists.");
            }
            if(await _context.Employees.AnyAsync(e=>e.SSN == employee.SSN))
            {
                return Conflict("SSN Must Be UNIQUE");
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/EmployeesTest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/soft-delete")]
        public IActionResult UpdateIsDeletedStatus(int id)
        {
            var existingEmployee = _context.Employees.Find(id);


            if (existingEmployee == null)
            {
                return NotFound();
            }


            existingEmployee.IsDeleted = true;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        [Route("api/attendance/{employeeId}/{startDate}/{endDate}")]
        public IActionResult GetAttendance(int employeeId, string startDate, string endDate)
        {
            try
            {
                if (!DateOnly.TryParse(startDate, out DateOnly parsedStartDate) ||
                    !DateOnly.TryParse(endDate, out DateOnly parsedEndDate))
                {
                    return BadRequest("Invalid date format. Please use a valid date format.");
                }

                var attendance = _context.EmployeesAttndens
                    .Where(a => a.empID == employeeId && a.Date >= parsedStartDate && a.Date <= parsedEndDate)
                    .ToList();

                if (attendance == null || !attendance.Any())
                {
                    return NotFound();
                }

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("api/attendance/{startDate}/{endDate}")]
        public IActionResult GetAllAttendance(string startDate, string endDate)
        {
            try
            {
                if (!DateOnly.TryParse(startDate, out DateOnly parsedStartDate) ||
                    !DateOnly.TryParse(endDate, out DateOnly parsedEndDate))
                {
                    return BadRequest("Invalid date format. Please use a valid date format.");
                }

                TimeOnly fixedDepartureTime = new TimeOnly(18, 0, 0);
                TimeOnly fixedAttendanceTime = new TimeOnly(9, 0, 0);

                var existAttendance = _context.EmployeesAttndens
                    .Where(a => a.Date >= parsedStartDate && a.Date <= parsedEndDate)
                    .ToList();

               

                foreach (var item in existAttendance)
                {
                    int plusHours = 0;
                    int lateHours = 0;

                    //if (item.Deperture > fixedDepartureTime)
                    //{
                    //    plusHours = (item.Deperture - fixedDepartureTime).Hours;
                    //}

                    //if (item.Deperture < fixedDepartureTime)
                    //{
                    //    lateHours = (item.Attendens - fixedAttendanceTime).Hours + (fixedDepartureTime - item.Deperture).Hours;
                    //}
                    if (item.Deperture > fixedDepartureTime)
                    {
                        plusHours = (item.Deperture - fixedDepartureTime).Hours;
                    }
                    else
                    {
                        lateHours =  (fixedDepartureTime - item.Deperture).Hours;
                    }


                    if (item.Attendens > fixedAttendanceTime)
                    {
                        lateHours += (item.Attendens - fixedAttendanceTime).Hours;
                    }

                    item.plus = plusHours;
                    item.late = lateHours;
                   

                    _context.Update(item);
                }

                _context.SaveChanges();

                var attendance = _context.EmployeesAttndens
                    .Where(a => a.Date >= parsedStartDate && a.Date <= parsedEndDate)
                    .Select(a => new DateAttendaceDTO
                    {
                        id = a.empID,
                        date = a.Date,
                        Attendens = a.Attendens,
                        Deperture = a.Deperture,
                        plus = a.plus,
                        late = a.late,                        
                        name = a.Employee.FullName
                    })
                    .ToList();

                if (attendance == null || !attendance.Any())
                {
                    return NotFound();
                }

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


 

        private bool EmployeeExists(int id)
             {
                return _context.Employees.Any(e => e.Id == id);
             }
    }
}
