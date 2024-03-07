namespace proj2.DTO
{
    public class SalariesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal salary { get; set; } 


        public List<MonthAttendanceDTO> Attendances { get; set; }
        
        public decimal netSalaryPerYear { get; set; }
        public int Totalplus { get; set; }
        public int Totallate { get; set; }

       // public int HoursWorked { get; set; }


    }

    public class MonthAttendanceDTO
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public int Plus { get; set; }
        public int Late { get; set; }
        public int days { get; set; }

        public decimal Salary {  get; set; }
    }
}
