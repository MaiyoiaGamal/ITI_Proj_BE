using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace proj2.Models
{
    public class EmployeeAttndens
    {
        [Key]
        public int id{ get; set; }
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Attendens { get; set; }
        [Required]
        public TimeOnly Deperture { get; set; }

        public int late { get; set; } = 0;

        public int plus { get; set; } = 0;


        [ForeignKey("Employee")]
        public int empID { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; } 


    }
}
