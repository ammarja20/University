using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Entities;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> e)
    {
        e.ToTable("Grades");
        e.HasKey(x => x.GradeId);


        e.Property(x => x.GradeValue)
         .HasColumnName("Grade")
         .HasColumnType("int");

        e.HasOne(g => g.Assignment)
         .WithMany(a => a.Grades)
         .HasForeignKey(g => g.AssignmentId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(g => g.Student)
         .WithMany(u => u.GradesReceived)
         .HasForeignKey(g => g.StudentId)
         .OnDelete(DeleteBehavior.Restrict);

    }
}
