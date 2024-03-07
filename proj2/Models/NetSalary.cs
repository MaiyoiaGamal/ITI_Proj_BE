using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace proj2.Models
{
    public class NetSalary
    {
        public int id { get; set; }

        public string month { get; set; }

        public string year { get; set; }

        public decimal netsalary { get; set; }

        [ForeignKey("employee")]
        public int employeeID { get; set; }

        [JsonIgnore]
        public Employee employee { get; set; }
    }
}
