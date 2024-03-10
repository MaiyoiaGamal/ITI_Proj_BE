namespace proj2.DTO
{
    public class NewSalaryDTO
    {
        public string emp_name { get; set; }
        public int Salary { get; set; }
        public int AttendaceDays { get; set; }
        public int AbsenceDays { get; set; }
        public int AddedHours { get; set; }
        public int lateHours { get; set; }
        public int AddedSalary { get; set; }
        public int SubtractedSalary { get; set; }
        public int OverallSalary { get; set; }
    }
}
