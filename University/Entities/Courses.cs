namespace University.Entities
{
    public class Courses
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public int? TeacherId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? SyllabusId { get; set; }

        public Users? Teacher { get; set; }
        public Syllabus? Syllabus { get; set; }   // one-to-one
        public List<Assignment> Assignments { get; set; } = new();
    }
}
