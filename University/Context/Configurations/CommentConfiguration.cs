using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Entities;

public class CommentConfiguration : IEntityTypeConfiguration<Comments>
{
    public void Configure(EntityTypeBuilder<Comments> e)
    {
        e.ToTable("Comments");
        e.HasKey(x => x.CommentId);

        e.Property(x => x.CommentContent).HasColumnType("nvarchar(max)");
        e.Property(x => x.CreatedDate).HasColumnType("datetime2").IsRequired();

     
        e.HasOne(c => c.Assignment)
         .WithMany(a => a.Comments)
         .HasForeignKey(c => c.AssignmentId)
         .OnDelete(DeleteBehavior.Cascade);   

        
        e.HasOne(c => c.CreatedByUser)
         .WithMany(u => u.CommentsCreated)
         .HasForeignKey(c => c.CreatedByUserId)
         .OnDelete(DeleteBehavior.Restrict);

    }
}
