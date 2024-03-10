using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace proj2.Models
{
    public class HRContext:IdentityDbContext<ApplicationUser>   
    {
        public HRContext() : base() { }
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<NetSalary> netSalaries { get; set; }
        public DbSet<EmployeeAttndens> EmployeesAttndens { get; set; }
        public DbSet<Setting> Settings { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-FIK5SPH\\SQLEXPRESS;Database=Hrproj;Trusted_connection=True;TrustServerCertificate=True;Integrated Security=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NetSalary>()
                .Property(ns => ns.netsalary)
                .HasColumnType("decimal(18,2)");
            base.OnModelCreating(modelBuilder);

            // Explicitly configure the primary key for IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(u => new { u.LoginProvider, u.ProviderKey });
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Explicitly configure the primary key for IdentityUserLogin
        //    modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(u => new { u.LoginProvider, u.ProviderKey });
        //}



        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Employee>()
        //       .HasOne(e => e.EmployeeAttndens)
        //       .WithOne(ea => ea.Employee)
        //       .HasForeignKey<EmployeeAttndens>(ea => ea.empID);
        //}
    }
}
