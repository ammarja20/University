namespace University.Entities
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public int? GradeValue { get; set; }

        public Assignment Assignment { get; set; } = null!;
        public Users Student { get; set; } = null!;
    }
}
