using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Entities;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> e)
    {
        e.ToTable("Assignments");
        e.HasKey(x => x.AssignmentId);

        e.Property(x => x.AssignmentTitle).HasMaxLength(128).IsRequired();
        e.Property(x => x.Description).HasColumnType("nvarchar(max)");
        e.Property(x => x.Weight).HasColumnType("float").IsRequired();
        e.Property(x => x.MaxGrade).IsRequired();
        e.Property(x => x.DueDate).HasColumnType("date").IsRequired();

       
        e.HasOne(a => a.Course)
         .WithMany(c => c.Assignments)
         .HasForeignKey(a => a.CourseId)
         .OnDelete(DeleteBehavior.Restrict);

    }
}
