using Microsoft.EntityFrameworkCore;
using University.Entities;

namespace University.Context
{
    public class UniversityDbContext : DbContext
    {
        public DbSet<Users> Users {  get; set; }
        public DbSet<Courses> Courses {  get; set; }
        public DbSet<Assignment> Assignments {  get; set; }
        public DbSet<Comments> Comments {  get; set; }
        public DbSet<Grade> Grades {  get; set; }
        public DbSet<Syllabus> Syllabus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniversityDbContext).Assembly);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=AMMAR\\SQLEXPRESS;Database=UniversityDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
}
