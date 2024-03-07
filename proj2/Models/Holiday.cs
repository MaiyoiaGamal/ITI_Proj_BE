using System.ComponentModel.DataAnnotations;

namespace proj2.Models
{
    public class Holiday
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
    }
}
