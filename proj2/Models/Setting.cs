using System.ComponentModel.DataAnnotations;

namespace proj2.Models
{
    public class Setting
    {
        //16-3s
        [Key]
        [Required] 
        public int Id { get; set; }
        [Required]
        [Range(1,20)]
        public int Plus { get; set; }
        [Required]
        [Range(1,20)]
        public int Late { get; set; }
        public DayOfWeek HolidayDayOne { get; set; } = DayOfWeek.Friday;
        public DayOfWeek HolidayDayTwo { get; set; } = DayOfWeek.Saturday;
    }
}
