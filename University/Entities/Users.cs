namespace University.Entities
{
    public class Users
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = null!; // "Student" / "Teacher"

        public List<Courses> CoursesTaught { get; set; } = new();   // if Role=Teacher
        public List<Comments> CommentsCreated { get; set; } = new(); // authored comments
        public List<Grade> GradesReceived { get; set; } = new();    // if Role=Student
    }
}
