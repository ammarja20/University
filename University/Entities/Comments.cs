namespace University.Entities
{
    public class Comments
    {
        public int CommentId { get; set; }
        public int AssignmentId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CommentContent { get; set; }

        public Assignment Assignment { get; set; } = null!;
        public Users CreatedByUser { get; set; } = null!;
    }
}
