using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using proj2.Validations;
using System.Text.Json.Serialization;
namespace proj2.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(14)]
        //[UniqueSSN]
        public string SSN { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(15)]
        public string Nationality { get; set; }
        [Required]
        [MaxLength(6)]
        public string Sex { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }

        [Required]
        public DateOnly ContractDate { get; set; }
        [Required]
        [Column(TypeName = "money")]
        [Range(2000, double.MaxValue, ErrorMessage = "Salary must be at least 2000")]
        public Decimal Salary { get; set; }

        public bool IsDeleted { get; set; } = false;   
        public List<EmployeeAttndens>? EmployeeAttndens { get; set; } = new List<EmployeeAttndens>();
    }
}
