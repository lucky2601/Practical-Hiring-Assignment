using Microsoft.EntityFrameworkCore;
using PracticalHiring.Models;

namespace PracticalHiring.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
