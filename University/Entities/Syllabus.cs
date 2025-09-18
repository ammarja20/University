namespace University.Entities
{
    public class Syllabus
    {
        public int SyllabusId { get; set; }
        public string? Description { get; set; }

        public Courses? Course { get; set; } 

    }
}
