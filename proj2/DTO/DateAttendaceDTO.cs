namespace proj2.DTO
{
    public class DateAttendaceDTO
    {
        public int id { get; set; }
        public string name { get; set; }

        public DateOnly date { get; set; }
        public TimeOnly Attendens { get; set; }
        public TimeOnly Deperture { get; set; }

        public int late { get; set; } = 0;

        public int plus { get; set; } = 0;
    }
}
