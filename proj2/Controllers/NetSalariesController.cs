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
    public class NetSalariesController : ControllerBase
    {
        private readonly HRContext _context;

        public NetSalariesController(HRContext context)
        {
            _context = context;
        }

        // GET: api/NetSalaries
        [HttpGet]
        //public List<SalariesDTO> CalculateSalaries()
        //{
        //    var employees = _context.Employees.ToList();
        //    var attendances = _context.EmployeesAttndens.ToList();
        //    List<SalariesDTO> salariesDTOs = new List<SalariesDTO>();

        //    foreach (var employee in employees)
        //    {
        //        SalariesDTO salaryDTO = new SalariesDTO
        //        {
        //            Id = employee.Id,
        //            Name = employee.FullName,

        //            Attendances = new List<MonthAttendanceDTO>()
        //        };

        //        decimal yearlyTotalSalary = 0;
        //        int currentYear = DateTime.Now.Year;

        //        for (int month = 1; month <= 12; month++)
        //        {
        //            var monthAttendance = attendances
        //                .Where(a => a.empID == employee.Id && a.Date.Month == month)
        //                .ToList();

        //            int plusTotal = monthAttendance.Sum(a => a.plus);
        //            int lateTotal = monthAttendance.Sum(a => a.late);
        //            salaryDTO.Totalplus += plusTotal;
        //            salaryDTO.Totallate += lateTotal;



        //            decimal monthlySalary = CalculateMonthlySalary(employee, plusTotal, lateTotal);
        //            yearlyTotalSalary += monthlySalary;

        //            int adjustedMonth = month;
        //            int adjustedYear = currentYear;

        //            if (adjustedMonth > 12)
        //            {
        //                adjustedYear++; // Move to the next year
        //            }

        //            salaryDTO.Attendances.Add(new MonthAttendanceDTO
        //            {
        //                Month =(month > 12 ? (month - 12).ToString() : month.ToString()),
        //                Year = adjustedYear,
        //                Plus = plusTotal,
        //                Late = lateTotal,
        //                Salary = monthlySalary,
        //            });
        //        }

        //        salaryDTO.netSalaryPerYear = yearlyTotalSalary;


        //        salariesDTOs.Add(salaryDTO);
        //    }


        //    return salariesDTOs;
        //}
        //public List<SalariesDTO> CalculateSalaries()
        //{
        //    var employees = _context.Employees.ToList();
        //    var attendances = _context.EmployeesAttndens.ToList();
        //    List<SalariesDTO> salariesDTOs = new List<SalariesDTO>();

        //    // Get the current year
        //    int currentYear = DateTime.Now.Year;

        //    foreach (var employee in employees)
        //    {
        //        for (int year = currentYear - 4; year <= currentYear + 10; year++) 
        //        {
        //            SalariesDTO salaryDTO = new SalariesDTO
        //            {
        //                Id = employee.Id,
        //                Name = employee.FullName,
        //                Attendances = new List<MonthAttendanceDTO>(),
        //                netSalaryPerYear = 0
        //            };

        //            for (int month = 1; month <= 12; month++)
        //            {
        //                var monthAttendance = attendances
        //                    .Where(a => a.empID == employee.Id && a.Date.Month == month && a.Date.Year == year)
        //                    .ToList();

        //                int plusTotal = monthAttendance.Sum(a => a.plus);
        //                int lateTotal = monthAttendance.Sum(a => a.late);
        //                salaryDTO.Totalplus += plusTotal;
        //                salaryDTO.Totallate += lateTotal;

        //                decimal monthlySalary = CalculateMonthlySalary(employee, plusTotal, lateTotal);
        //                salaryDTO.netSalaryPerYear += monthlySalary;

        //                salaryDTO.Attendances.Add(new MonthAttendanceDTO
        //                {
        //                    Month = month.ToString(),
        //                    Year = year,
        //                    Plus = plusTotal,
        //                    Late = lateTotal,
        //                    Salary = monthlySalary
        //                });
        //            }

        //            salariesDTOs.Add(salaryDTO);
        //        }
        //    }

        //    return salariesDTOs;
        //}

        public List<SalariesDTO> CalculateSalaries()
        {
            var employees = _context.Employees.ToList();
            var attendances = _context.EmployeesAttndens.ToList();
            List<SalariesDTO> salariesDTOs = new List<SalariesDTO>();

            // Get the current year
            int currentYear = DateTime.Now.Year;

            for (int year = currentYear - 4; year <= currentYear + 10; year++) // 2020 bdaet el sherka to 2034
            {
                foreach (var employee in employees)
                {
                    SalariesDTO salaryDTO = new SalariesDTO
                    {
                        Id = employee.Id,
                        Name = employee.FullName,
                        salary = employee.Salary,
                        Attendances = new List<MonthAttendanceDTO>(),
                        netSalaryPerYear = 0
                    };

                    var monthsWithAttendance = Enumerable.Range(1, 12)
                        .Where(month => attendances.Any(a => a.empID == employee.Id && a.Date.Month == month && a.Date.Year == year))
                        .ToList();

                    if (monthsWithAttendance.Any())
                    {
                        foreach (var month in monthsWithAttendance)
                        {
                            var monthAttendance = attendances
                                .Where(a => a.empID == employee.Id && a.Date.Month == month && a.Date.Year == year)
                                .ToList();

                            int plusTotal = monthAttendance.Sum(a => a.plus);
                            int lateTotal = monthAttendance.Sum(a => a.late);
                            //int daysTotal = monthAttendance.Sum(a => a.days);
                            int daysTotal = monthAttendance.Select(a => a.Date.ToDateTime(new TimeOnly(0, 0))).Distinct().Count();
                            salaryDTO.Totalplus += plusTotal;
                            salaryDTO.Totallate += lateTotal;

                            decimal monthlySalary = CalculateMonthlySalary(employee, plusTotal, lateTotal, daysTotal);
                            salaryDTO.netSalaryPerYear += monthlySalary;

                            salaryDTO.Attendances.Add(new MonthAttendanceDTO
                            {
                                Month = month.ToString(),
                                Year = year,
                                Plus = plusTotal,
                                Late = lateTotal,
                                Salary = monthlySalary,
                                days = daysTotal
                            });
                        }

                        salariesDTOs.Add(salaryDTO);
                    }
                }
            }

            return salariesDTOs;
        }


        //[HttpGet("{startDate}/{endDate}")]
        //public IActionResult GetEmployeesReports(string startDate, string endDate)
        //{
        //    try
        //    {
        //        if (!DateOnly.TryParse(startDate, out DateOnly parsedStartDate) ||
        //            !DateOnly.TryParse(endDate, out DateOnly parsedEndDate))
        //        {
        //            return BadRequest("Invalid date format. Please use a valid date format.");
        //        }

        //        var employees = _context.Employees.ToList();
        //        var attendances = _context.EmployeesAttndens
        //            .Where(a => a.Date >= parsedStartDate && a.Date <= parsedEndDate)
        //            .ToList();

        //            if (attendances == null || !attendances.Any())
        //            {
        //                return NotFound();
        //            }

        //        List<SalariesDTO> salariesDTOs = new List<SalariesDTO>();

        //        foreach (var employee in employees)
        //        {
        //            SalariesDTO salaryDTO = new SalariesDTO
        //            {
        //                Id = employee.Id,
        //                Name = employee.FullName,
        //                salary = employee.Salary,
        //                Attendances = new List<MonthAttendanceDTO>(),
        //                netSalaryPerYear = 0
        //            };

        //            var monthsWithAttendance = attendances
        //                .Where(a => a.empID == employee.Id)
        //                .Select(a => new { a.Date.Year, a.Date.Month })
        //                .Distinct()
        //                .ToList();

        //            if (monthsWithAttendance.Any())
        //            {
        //                foreach (var monthAttendance in monthsWithAttendance)
        //                {
        //                    var attendanceForMonth = attendances
        //                        .Where(a => a.empID == employee.Id && a.Date.Month == monthAttendance.Month && a.Date.Year == monthAttendance.Year)
        //                        .ToList();

        //                    int plusTotal = attendanceForMonth.Sum(a => a.plus);
        //                    int lateTotal = attendanceForMonth.Sum(a => a.late);
        //                    int daysTotal = attendanceForMonth.Select(a => a.Date.ToDateTime(new TimeOnly(0, 0))).Distinct().Count();

        //                    salaryDTO.Totalplus += plusTotal;
        //                    salaryDTO.Totallate += lateTotal;

        //                    decimal monthlySalary = CalculateMonthlySalary(employee, plusTotal, lateTotal);
        //                    salaryDTO.netSalaryPerYear += monthlySalary;

        //                    salaryDTO.Attendances.Add(new MonthAttendanceDTO
        //                    {
        //                        Month = monthAttendance.Month.ToString(),
        //                        Year = monthAttendance.Year,
        //                        Plus = plusTotal,
        //                        Late = lateTotal,
        //                        Salary = monthlySalary,
        //                        days = daysTotal
        //                    });
        //                }

        //                salariesDTOs.Add(salaryDTO);
        //            }
        //        }

        //        return Ok(salariesDTOs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpGet("{startDate}/{endDate}")]
        public IActionResult GetEmployeesReports(string startDate, string endDate)
        {
            try
            {
                if (!DateOnly.TryParse(startDate, out DateOnly parsedStartDate) ||
                    !DateOnly.TryParse(endDate, out DateOnly parsedEndDate))
                {
                    return BadRequest("Invalid date format. Please use a valid date format.");
                }

                var employees = _context.Employees.ToList();
                var attendances = _context.EmployeesAttndens
                    .Where(a => a.Date >= parsedStartDate && a.Date <= parsedEndDate)
                    .ToList();

                if (attendances == null || !attendances.Any())
                {
                    return NotFound();
                }

                List<SalariesDTO> salariesDTOs = new List<SalariesDTO>();

                foreach (var employee in employees)
                {
                    SalariesDTO salaryDTO = new SalariesDTO
                    {
                        Id = employee.Id,
                        Name = employee.FullName,
                        salary = employee.Salary,
                        Attendances = new List<MonthAttendanceDTO>(),
                        netSalaryPerYear = 0
                    };

                    // Get all months within the date range
                    var monthsInRange = Enumerable.Range(parsedStartDate.Month, parsedEndDate.Month - parsedStartDate.Month + 1);

                    foreach (var month in monthsInRange)
                    {
                        var year = parsedStartDate.Year;

                        var attendanceForMonth = attendances
                            .Where(a => a.empID == employee.Id && a.Date.Month == month && a.Date.Year == year)
                            .ToList();

                        int plusTotal = attendanceForMonth.Sum(a => a.plus);
                        int lateTotal = attendanceForMonth.Sum(a => a.late);

                        // Calculate the total days in the month
                        int daysInMonth = DateTime.DaysInMonth(year, month);

                        // Calculate the number of off days (Saturdays, Sundays, and holidays)
                        int offDays = GetOffDays(year, month, daysInMonth);

                        // Calculate the number of working days
                        int workingDays = daysInMonth - offDays;

                        // Calculate the salary for attended days
                        decimal monthlySalary = CalculateMonthlySalary(employee, plusTotal, lateTotal, workingDays);

                        salaryDTO.Totalplus += plusTotal;
                        salaryDTO.Totallate += lateTotal;
                        salaryDTO.netSalaryPerYear += monthlySalary;

                        salaryDTO.Attendances.Add(new MonthAttendanceDTO
                        {
                            Month = month.ToString(),
                            Year = year,
                            Plus = plusTotal,
                            Late = lateTotal,
                            Salary = monthlySalary,
                            days = workingDays
                        });
                    }

                    salariesDTOs.Add(salaryDTO);
                }
                
                return Ok(salariesDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int GetOffDays(int year, int month, int daysInMonth)
        {
            // Calculate the number of Saturdays and Sundays
            int saturdays = Enumerable.Range(1, daysInMonth)
                                       .Count(day => new DateTime(year, month, day).DayOfWeek == DayOfWeek.Saturday);
            int sundays = Enumerable.Range(1, daysInMonth)
                                     .Count(day => new DateTime(year, month, day).DayOfWeek == DayOfWeek.Sunday);

            // Assuming 8 holidays per month
            int holidays = 8;

            return saturdays + sundays + holidays;
        }

        private decimal CalculateMonthlySalary(Employee employee, int plusTotal, int lateTotal, int workingDays)
        {
            // Ensure working days doesn't go negative
            workingDays = Math.Max(workingDays, 0);

            // Calculate the total salary for the month
            decimal salaryPerDay = employee.Salary / workingDays;
            decimal totalSalary = (workingDays - lateTotal) * salaryPerDay;

            // Ensure the calculated salary is not negative
            decimal monthlySalary = Math.Max(totalSalary, 0);

            return monthlySalary;
        }





        // GET: api/NetSalaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NetSalary>> GetNetSalary(int id)
        {
            var netSalary = await _context.netSalaries.FindAsync(id);

            if (netSalary == null)
            {
                return NotFound();
            }

            return netSalary;
        }

        // PUT: api/NetSalaries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetSalary(int id, NetSalary netSalary)
        {
            if (id != netSalary.id)
            {
                return BadRequest();
            }

            _context.Entry(netSalary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetSalaryExists(id))
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

        // POST: api/NetSalaries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NetSalary>> PostNetSalary(NetSalary netSalary)
        {
            _context.netSalaries.Add(netSalary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNetSalary", new { id = netSalary.id }, netSalary);
        }

        // DELETE: api/NetSalaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNetSalary(int id)
        {
            var netSalary = await _context.netSalaries.FindAsync(id);
            if (netSalary == null)
            {
                return NotFound();
            }

            _context.netSalaries.Remove(netSalary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NetSalaryExists(int id)
        {
            return _context.netSalaries.Any(e => e.id == id);
        }
    }
}
