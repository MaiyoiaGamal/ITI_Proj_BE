using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proj2.Models;
using proj2.Repos;
 

namespace proj2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {

        private readonly HRContext hRContext;
        private readonly SalaryRepo salaryrepo;


        public SalaryController( HRContext db , SalaryRepo salaryrepo)
        {
            this.salaryrepo = salaryrepo;
            hRContext = db;
        }


        [HttpGet]
        public IActionResult GetEmployee()
        {
            return Ok(hRContext.Employees.ToList());
        }

        [HttpGet("{empId:int}/{Year:int}/{Month:int}")]
        public IActionResult GetSales(int empId, int Year, int Month)
        {
            return Ok(salaryrepo.GetSalaryforEmp(empId, Year, Month));
        }
    }
}
