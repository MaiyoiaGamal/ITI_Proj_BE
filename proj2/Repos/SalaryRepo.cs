using proj2.DTO;
using proj2.Models;
using System;

namespace proj2.Repos
{
    public class SalaryRepo
    {
        private readonly HRContext db;

        public SalaryRepo(HRContext db)
        {
            this.db = db;
        }
        public NewSalaryDTO GetSalaryforEmp(int empId, int year, int month)
        {

            Employee emp = db.Employees.Where(e => e.Id == empId).SingleOrDefault();

            TimeOnly emp_arrival = new TimeOnly(9, 00, 00); //TimeOnly.Parse($"{emp.star}"); 
            TimeOnly emp_dismissal = new TimeOnly(18, 00, 00); //TimeOnly.Parse($"{emp.EndTime}");

            #region num of days & hoidays
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int OfficialHolidays = this.CountofHolidays(year, month , db.Holidays.Select(h => h.Date).ToList());
            int FirstWeekend = this.CountDayOfWeekInMonth(year, month ,(int) db.Settings.Select(s => s.HolidayDayOne).SingleOrDefault());
            int SecondWeekend = this.CountDayOfWeekInMonth(year, month, (int)db.Settings.Select(s => s.HolidayDayTwo).SingleOrDefault());
            int Requiredattendance = daysInMonth - (OfficialHolidays + FirstWeekend + SecondWeekend );
            //int Requiredattendance = daysInMonth - ( FirstWeekend + SecondWeekend );

            #endregion


            #region hours late and overtime
            var logs = db.EmployeesAttndens.Where(e => e.Date.Year == year && e.Date.Month == month && e.empID == empId).ToList();
            int addedhours = 0;
            int subtractedhours = 0;
            foreach (var item in logs)
            {
                addedhours += (this.SubtractTimes(item.Deperture, emp_dismissal));
                subtractedhours += (this.SubtractTimes(emp_arrival, item.Attendens));
            }
            #endregion 


            #region salary
            float dayRate = this.RateOfSalaryPerDay((float)(emp.Salary), Requiredattendance);
            float hourRate = this.RateofSalaryPerHour((float)(emp.Salary), Requiredattendance, emp_dismissal, emp_arrival);
            //genral settings 
            var genrallate = db.Settings.Select(s => s.Late).FirstOrDefault();
            var genralplus = db.Settings.Select(s => s.Plus).FirstOrDefault();

            float genralholidydayssalary = 0;
             genralholidydayssalary = OfficialHolidays * dayRate;



            float addedsalary = addedhours * hourRate * genralplus ;
            float subsalary = subtractedhours * hourRate * genrallate;
            float salary = (float)emp.Salary;

            float Salary = (salary - (dayRate * (Requiredattendance - logs.Count())) + addedsalary - Math.Abs(subsalary));


            #endregion
            

            #region return DTO
            NewSalaryDTO reportDTO = new NewSalaryDTO()
            {
                emp_name = emp.FullName,
                Salary = (int)emp.Salary,
                OverallSalary =(int) Salary + (int) genralholidydayssalary,
                AttendaceDays = logs.Count(),
                AbsenceDays = Requiredattendance - logs.Count(),
                AddedHours = addedhours,
                lateHours = subtractedhours,
                AddedSalary =(int) addedsalary,
                SubtractedSalary =(int) subsalary
            };
            return reportDTO;
            #endregion


        }

        private int CountDayOfWeekInMonth(int year, int month, int weekend)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), $"{weekend}");
            DateTime startDate = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            int weekDayCount = 0;
            for (int day = 0; day < days; ++day)
            {
                weekDayCount += startDate.AddDays(day).DayOfWeek == dayOfWeek ? 1 : 0;
            }
            return weekDayCount;
        }


        private int CountofHolidays(int year, int month, List<DateOnly> dates)
        {
            int count = 0;
            foreach (DateOnly dateOnly in dates)
            {
                if (dateOnly.Year == year && dateOnly.Month == month)
                {
                    count++;
                }
            }
            return count;
        }

        private float RateOfSalaryPerDay(float Salary, float CountofDays)
        {
            return Salary / CountofDays;
        }

        private float RateofSalaryPerHour(float Salary, float CountofDays, TimeOnly Dissmisal, TimeOnly Arrival)
        {
            return Salary / (CountofDays * this.SubtractTimes(Dissmisal, Arrival));
        }

        private int SubtractTimes(TimeOnly t1, TimeOnly t2)
        {
            int HourT1 = 0;
            if (t1.Minute != 0)
            {
                HourT1 = t1.Hour + 1;
            }
            else
            {
                HourT1 = t1.Hour;
            }
            int HourT2 = 0;
            if (t2.Minute != 0)
            {
                HourT2 = t2.Hour + 1;
            }
            else
            {
                HourT2 = t2.Hour;
            }
            return HourT1 - HourT2;
        }

        //private int SubtractTimes(TimeOnly t1, TimeOnly t2)
        //{
        //    int HourT1 = t1.Hour;
        //    if (t1.Minute != 0)
        //    {
        //        HourT1 += 1;
        //    }

        //    int HourT2 = t2.Hour;
        //    if (t2.Minute != 0)
        //    {
        //        HourT2 += 1;
        //    }

        //    return HourT1 - HourT2;
        //}


    }
}

