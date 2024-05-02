using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PracticeMAUI.API2.Model
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }

    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Joindate { get; set; }
        public int Salary { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
    }
}
