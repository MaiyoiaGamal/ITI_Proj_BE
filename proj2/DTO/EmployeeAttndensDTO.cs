using System.ComponentModel.DataAnnotations;

namespace proj2.DTO
{
    public class EmployeeAttndensDTO
    {
        public int id { get; set; }

        public string name { get; set; }   

        //public TimeOnly fixedAttendse { get; } = new TimeOnly(09, 00, 00);
        //public TimeOnly fixedDeperture { get; } = new TimeOnly(18, 00, 00);

        public List<ListOfAttendes>? ListOfAttendes { get; set; }
       
    }
}
