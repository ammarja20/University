using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context
{
    public class UniversityDbContextFactory : IDesignTimeDbContextFactory<UniversityDbContext>
    {
        public UniversityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniversityDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=AMMAR\\SQLEXPRESS;Database=UniversitySystem;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new UniversityDbContext(optionsBuilder.Options);
        }
    }

}
