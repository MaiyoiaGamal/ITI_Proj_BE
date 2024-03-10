namespace proj2.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public DayOfWeek HolidayDayOne { get; set; } = DayOfWeek.Friday;
        public DayOfWeek HolidayDayTwo { get; set; } = DayOfWeek.Saturday;
    }
}
