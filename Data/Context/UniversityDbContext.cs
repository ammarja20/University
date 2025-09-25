using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
            : base(options) { }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
    }
}
