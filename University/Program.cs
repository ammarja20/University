using Microsoft.EntityFrameworkCore;
using University.Context;
using University.Entities;

namespace University;

public class Program
{
    private static async Task Main()
    {
        using var db = new UniversityDbContext();

        // ===== Ensure schema is up to date >>> If the DB doesn’t exist, it creates it. If migrations exist, it applies them. =====
        await db.Database.MigrateAsync();

        // ===== Seed once : If the Users table is empty, it calls SeedAsync() to insert initial data (students, teachers, courses, etc.).
        // If data exists, it skips → avoids duplicates. ======
        if (!await db.Users.AnyAsync())
        {
            await SeedAsync(db);
            Console.WriteLine("Seed complete.");
        }
        else
        {
            Console.WriteLine("Seed skipped (data exists).");
        }

      
        await RunQueriesAsync(db);


        await RunUpdatesAndDeletesAsync(db);

        Console.WriteLine("\nAll done.");
    }

    // ------------------------------
    // ===== Seed data =====
    // ------------------------------


    private static async Task SeedAsync(UniversityDbContext db)
    {
        var rand = new Random(42);
        var today = DateTime.Today;

        // === Users: interns (students) + teachers (Sami, Feryal) ===
        var users = new List<Users>
        {
            new() { UserName="Ammar.Ja",   FirstName="Ammar",   LastName="Jamous",   EmailAddress="ammar.ja.20@example.com",   PhoneNumber="5551111111", Role="Student" },
            new() { UserName="Alaa", FirstName="Alaa", LastName="Abuyahia",  EmailAddress="alaa.abuyahia@example.com",PhoneNumber="5551111112", Role="Student" },
            new() { UserName="Mohammed",  FirstName="Mohammed",  LastName="Kourdia",  EmailAddress="mohammed@example.com", PhoneNumber="5551111113", Role="Student" },
            new() { UserName="Shahad",  FirstName="Shahad",  LastName="Totah", EmailAddress="shahad@example.com",PhoneNumber="5551111114", Role="Student" },
            new() { UserName="hadi.b",  FirstName="Hadi",  LastName="Barakat",EmailAddress="hadi.barakat@example.com",PhoneNumber="5551111115", Role="Student" },
            new() { UserName="lina.s",  FirstName="Lina",  LastName="Saleh",  EmailAddress="lina.saleh@example.com", PhoneNumber="5551111116", Role="Student" },
            new() { UserName="yusuf.ak",FirstName="Yusuf", LastName="Ak",     EmailAddress="yusuf.ak@example.com",   PhoneNumber="5551111117", Role="Student" },
            new() { UserName="maya.h",  FirstName="Maya",  LastName="Hassan", EmailAddress="maya.hassan@example.com",PhoneNumber="5551111118", Role="Student" },

            new() { UserName="sami.h",  FirstName="Sami",  LastName="Hijazi", EmailAddress="sami.hijazi@example.com",PhoneNumber="5552222221", Role="Teacher" },
            new() { UserName="feryal.a",FirstName="Feryal",LastName="Aslan",  EmailAddress="feryal.aslan@example.com",PhoneNumber="5552222222", Role="Teacher" },
        };
        db.Users.AddRange(users);
        await db.SaveChangesAsync();

        var samiId = users.Single(u => u.UserName == "sami.h").UserId;
        var feryalId = users.Single(u => u.UserName == "feryal.a").UserId;

        // === Syllabus (one per course) ===
        var syllabi = new List<Syllabus>
        {
            new() { Description = "Syllabus for SQL" },
            new() { Description = "Syllabus for C#" },
            new() { Description = "Syllabus for Entity Framework" },
            new() { Description = "Syllabus for Web API" },
            new() { Description = "Syllabus for React" }
        };
        db.Syllabus.AddRange(syllabi);
        await db.SaveChangesAsync();

        // === Courses (5) assigned to teachers + linked syllabus ===
        var courses = new List<Courses>
        {
            new() { CourseName="SQL",              TeacherId=samiId,   StartDate=today.AddDays(-7), EndDate=today.AddMonths(2), SyllabusId=syllabi[0].SyllabusId },
            new() { CourseName="C#",               TeacherId=samiId,   StartDate=today,             EndDate=today.AddMonths(2), SyllabusId=syllabi[1].SyllabusId },
            new() { CourseName="Entity Framework", TeacherId=samiId,   StartDate=today.AddDays(3),  EndDate=today.AddMonths(2), SyllabusId=syllabi[2].SyllabusId },
            new() { CourseName="Web API",          TeacherId=feryalId, StartDate=today.AddDays(7),  EndDate=today.AddMonths(2), SyllabusId=syllabi[3].SyllabusId },
            new() { CourseName="React",            TeacherId=feryalId, StartDate=today.AddDays(10), EndDate=today.AddMonths(2), SyllabusId=syllabi[4].SyllabusId },
        };
        db.Courses.AddRange(courses);
        await db.SaveChangesAsync();

        // === Assignments: 5 per course with mixed past/future due dates ===
        var assignments = new List<Assignment>();
        foreach (var c in courses)
        {
            for (int i = 1; i <= 5; i++)
            {
                assignments.Add(new Assignment
                {
                    CourseId = c.CourseId,
                    AssignmentTitle = $"{c.CourseName} Assignment {i}",
                    Description = $"Work for {c.CourseName} #{i}",
                    Weight = 20,          // total per course = 100 5x20=100
                    MaxGrade = 100,
                    DueDate = today.AddDays(rand.Next(-20, 20))
                });
            }
        }
        db.Assignments.AddRange(assignments);
        await db.SaveChangesAsync();

        // === Comments: 10 random ===
        var studentIds = users.Where(u => u.Role == "Student").Select(u => u.UserId).ToList();
        var assignmentIds = assignments.Select(a => a.AssignmentId).ToList();
        for (int i = 1; i <= 10; i++)
        {
            db.Comments.Add(new Comments
            {
                AssignmentId = assignmentIds[rand.Next(assignmentIds.Count)],
                CreatedByUserId = studentIds[rand.Next(studentIds.Count)],
                CreatedDate = DateTime.Now.AddMinutes(-rand.Next(10_000)),
                CommentContent = $"Comment #{i}"
            });
        }
        await db.SaveChangesAsync();

        // === Grades: every student for every assignment ===
        foreach (var sid in studentIds)
        {
            foreach (var a in assignments)
            {
                db.Grades.Add(new Grade
                {
                    AssignmentId = a.AssignmentId,
                    StudentId = sid,
                    GradeValue = rand.Next(60, 101)
                });
            }
        }
        await db.SaveChangesAsync();
    }

    // quick counts after seeding
    private static async Task VerifySeedAsync(UniversityDbContext db)
    {
        var users = await db.Users.CountAsync();
        var students = await db.Users.CountAsync(u => u.Role == "Student");
        var teachers = await db.Users.CountAsync(u => u.Role == "Teacher");
        var courses = await db.Courses.CountAsync();
        var syllabi = await db.Syllabus.CountAsync();
        var assigns = await db.Assignments.CountAsync();
        var comments = await db.Comments.CountAsync();
        var grades = await db.Grades.CountAsync();
        Console.WriteLine($"\nSeed check: Users={users} (S:{students}/T:{teachers}), Courses={courses}, Syllabi={syllabi}, Assignments={assigns}, Comments={comments}, Grades={grades}");
    }

    // ------------------------------
    // === Queries + helpers ===
    // ------------------------------
    private static async Task RunQueriesAsync(UniversityDbContext db)
    {
        // 1) List all courses
        Console.WriteLine("\nAll courses:");
        var allCourses = await db.Courses
            .Select(c => new { c.CourseId, c.CourseName, c.StartDate, c.EndDate })
            .ToListAsync();
        foreach (var c in allCourses)
            Console.WriteLine($" - {c.CourseId}: {c.CourseName} ({c.StartDate:d} → {c.EndDate:d})");

        // 2) Show all assignments for a specific course
        var courseName = "Entity Framework";
        Console.WriteLine($"\nAssignments for '{courseName}':");
        var assignmentsForCourse = await db.Assignments
            .Where(a => a.Course.CourseName == courseName)
            .OrderBy(a => a.DueDate)
            .Select(a => new { a.AssignmentId, a.AssignmentTitle, a.DueDate })
            .ToListAsync();
        foreach (var a in assignmentsForCourse)
            Console.WriteLine($" - #{a.AssignmentId} {a.AssignmentTitle} (Due {a.DueDate:d})");

        // 3) List all students
        Console.WriteLine("\nStudents:");
        var students = await db.Users
            .Where(u => u.Role == "Student")
            .Select(u => new { u.UserId, u.FirstName, u.LastName })
            .ToListAsync();
        foreach (var s in students)
            Console.WriteLine($" - {s.UserId}: {s.FirstName} {s.LastName}");

        // 4) Show all comments for a given assignment (first assignment)
        Console.WriteLine("\nComments for the first assignment of that course:");
        var targetAssignmentId = assignmentsForCourse.FirstOrDefault()?.AssignmentId;
        if (targetAssignmentId != null)
        {
            var comments = await db.Comments
                .Where(c => c.AssignmentId == targetAssignmentId)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new
                {
                    c.CommentId,
                    c.CreatedDate,
                    c.CommentContent,
                    Author = c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName
                })
                .ToListAsync();

            if (comments.Count == 0) Console.WriteLine(" (no comments yet)");
            foreach (var c in comments)
                Console.WriteLine($" - [{c.CreatedDate:g}] #{c.CommentId} {c.Author}: {c.CommentContent}");
        }
        else
        {
            Console.WriteLine(" (no assignments found for that course)");
        }

        // 5) Show all grades for a student (first student)
        //FirstOrDefaultAsync : Returns the first matching element in the sequence.
        var firstStudent = await db.Users.Where(u => u.Role == "Student").FirstOrDefaultAsync();
        if (firstStudent != null)
        {
            Console.WriteLine($"\nGrades for {firstStudent.FirstName} {firstStudent.LastName}:");
            var sGrades = await db.Grades
                .Where(g => g.StudentId == firstStudent.UserId)
                .Select(g => new
                {
                    Course = g.Assignment.Course.CourseName,
                    Assignment = g.Assignment.AssignmentTitle,
                    Score = g.GradeValue ?? 0
                })
                .ToListAsync();

            foreach (var g in sGrades)
                Console.WriteLine($" - {g.Course} / {g.Assignment}: {g.Score} ({ToLetter(g.Score)})");

            // 8) GPA for that student
            var gpa = await CalculateGpaAsync(db, firstStudent.UserId);
            Console.WriteLine($"\nGPA for {firstStudent.FirstName} {firstStudent.LastName}: {gpa:F2}");
        }

        // 6) Each assignment with its course and the teacher’s full name
        Console.WriteLine("\nAssignment | Course | Teacher:");
        var act = await db.Assignments
            .Select(a => new
            {
                a.AssignmentTitle,
                Course = a.Course.CourseName,
                Teacher = a.Course.Teacher == null
                        ? "(No teacher)"
                        : a.Course.Teacher.FirstName + " " + a.Course.Teacher.LastName
            })
            .ToListAsync();
        foreach (var x in act)
            Console.WriteLine($" - {x.AssignmentTitle} | {x.Course} | {x.Teacher}");

        // 7) Average grade per course
        Console.WriteLine("\nAverage grade per course:");
        var avgPerCourse = await db.Courses
            .Select(c => new
            {
                c.CourseName,
                Avg = c.Assignments
                        .SelectMany(a => a.Grades)
                        .Average(g => (double)(g.GradeValue ?? 0))
            })
            .OrderByDescending(x => x.Avg)
            .ToListAsync();
        foreach (var x in avgPerCourse)
            Console.WriteLine($" - {x.CourseName}: {x.Avg:F2}");
    }

    // ===== letter grades & GPA =====
    private static string ToLetter(int score)
    {
        if (score >= 90) return "A";
        if (score >= 80) return "B";
        if (score >= 70) return "C";
        if (score >= 60) return "D";
        return "F";
    }

    private static double LetterToPoints(string letter) => letter switch
    {
        "A" => 4.0,
        "B" => 3.0,
        "C" => 2.0,
        "D" => 1.0,
        _ => 0.0
    };

    private static async Task<double> CalculateGpaAsync(UniversityDbContext db, int studentId)
    {
        var scores = await db.Grades
            .Where(g => g.StudentId == studentId)
            .Select(g => g.GradeValue ?? 0)
            .ToListAsync();

        if (!scores.Any()) return 0.0;

        var points = scores.Select(s => LetterToPoints(ToLetter(s)));
        return points.Average();
    }

    // ------------------------------
    // ==== Updates & Deletions ====
    // ------------------------------
    private static async Task RunUpdatesAndDeletesAsync(UniversityDbContext db)
    {
        Console.WriteLine("\n=== STEP 4: Updates & Deletions ===");

        // Update: promote one Student to Teacher
        var student = await db.Users
            .Where(u => u.Role == "Student")
            .OrderBy(u => u.UserId)
            .FirstOrDefaultAsync();

        if (student != null)
        {
            student.Role = "Teacher";
            await db.SaveChangesAsync();
            Console.WriteLine($"Promoted user #{student.UserId} ({student.FirstName} {student.LastName}) to Teacher.");
        }
        else
        {
            Console.WriteLine("No users with Role='Student' found to promote.");
        }

        // Delete: remove a specific comment
         int commentIdToDelete = 1;
         var target = await db.Comments.FindAsync(commentIdToDelete);
    }
}
