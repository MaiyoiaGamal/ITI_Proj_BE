using proj2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace proj2.DTO
{
    public class EmployeeDTO
    {
       
        public int Id { get; set; }
        public string FullName { get; set; }
        public string SSN { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string Sex { get; set; }
        public String Email { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateOnly ContractDate { get; set; }
        public Decimal Salary { get; set; }
        public bool IsDeleted { get; set; } = false;

        //public DateOnly? Date {  get; set; }
        //public TimeOnly? Attendens { get; set; }
        //public TimeOnly? Deperture { get; set; }

        public EmployeeAttndensDTO? EmployeeAttndens { get; set; }
    }
}
