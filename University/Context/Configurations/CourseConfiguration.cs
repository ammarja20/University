using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Entities;

public class CourseConfiguration : IEntityTypeConfiguration<Courses>
{
    public void Configure(EntityTypeBuilder<Courses> e)
    {
        e.ToTable("Courses");
        e.HasKey(x => x.CourseId);

        e.Property(x => x.CourseName).HasMaxLength(100).IsRequired();
        e.Property(x => x.StartDate).HasColumnType("datetime2").IsRequired();
        e.Property(x => x.EndDate).HasColumnType("datetime2").IsRequired();

       
        e.HasOne(c => c.Teacher)
         .WithMany(u => u.CoursesTaught)
         .HasForeignKey(c => c.TeacherId)
         .OnDelete(DeleteBehavior.Restrict);

        
        e.HasOne(c => c.Syllabus)
         .WithOne(s => s.Course)
         .HasForeignKey<Courses>(c => c.SyllabusId)
         .OnDelete(DeleteBehavior.SetNull);

        e.HasIndex(c => c.SyllabusId)
         .IsUnique()
         .HasFilter("[SyllabusId] IS NOT NULL");

    }
}
