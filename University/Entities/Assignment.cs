namespace University.Entities
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public int CourseId { get; set; }
        public string AssignmentTitle { get; set; } = null!;
        public string? Description { get; set; }
        public double Weight { get; set; }
        public int MaxGrade { get; set; }
        public DateTime DueDate { get; set; }

        public Courses Course { get; set; } = null!; //1:1
        public List<Comments> Comments { get; set; } = new();//1:many
        public List<Grade> Grades { get; set; } = new();
    }
}
