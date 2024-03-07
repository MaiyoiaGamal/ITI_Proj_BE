using Microsoft.EntityFrameworkCore;

namespace proj2.Models
{
    public class HRContext:DbContext    
    {
        public HRContext() : base() { }
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<NetSalary> netSalaries { get; set; }
        public DbSet<EmployeeAttndens> EmployeesAttndens { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-FIK5SPH\\SQLEXPRESS;Database=Hrproj;Trusted_connection=True;TrustServerCertificate=True;Integrated Security=True");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NetSalary>()
                .Property(ns => ns.netsalary)
                .HasColumnType("decimal(18,2)");
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Employee>()
        //       .HasOne(e => e.EmployeeAttndens)
        //       .WithOne(ea => ea.Employee)
        //       .HasForeignKey<EmployeeAttndens>(ea => ea.empID);
        //}
    }
}
