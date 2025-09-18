using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Entities;

public class SyllabusConfiguration : IEntityTypeConfiguration<Syllabus>
{
    public void Configure(EntityTypeBuilder<Syllabus> e)
    {
        e.ToTable("Syllabus");
        e.HasKey(x => x.SyllabusId);
        e.Property(x => x.Description).HasColumnType("nvarchar(max)");

    }
}
