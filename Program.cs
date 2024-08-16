using EFCore2.Models;
using Microsoft.EntityFrameworkCore;

using var db = new AppDbContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

Professor KareemDana = new Professor
{
    FirstName = "Kareem",
    LastName = "Dana",
    Courses = new List<Course>
    {
        new Course { Department = "CIDM", Number = 4385, Name = "Mobile Application Development"},
        new Course { Department = "Business", Number = 3320, Name = "Monetizing Personal Date with AI"}
    }
};

Professor professorOne = new Professor { FirstName = "Sean", LastName = "Humpherys"};
Professor professorTwo = new Professor { FirstName = "Carl", LastName = "Zheng"};
Professor professorThree = new Professor { FirstName = "Murray", LastName = "Jennex"};

List<Course> listOfCourses = new List<Course>() {
    new Course { Department = "CIDM", Number = 2315, Name = "Programming Business Applications", Professor = professorTwo},
    new Course { Department = "CIDM", Number = 3312, Name = "Advanced Business Programming", Professor = professorOne},
    new Course { Department = "CIDM", Number = 3350, Name = "Database Systems Design", Professor = professorThree},
    new Course { Department = "Physics", Number = 6101, Name = "Bioelectronic Systems and Neural Implants", Professor = KareemDana},
    new Course { Department = "Physics", Number = 6395, Name = "Quantum Mechanics in Neural Engineering", Professor = professorTwo},
    new Course { Department = "Cybernetics", Number = 6450, Name = "Synthetic Cognition: Building Conscious Machines", Professor = professorTwo},
    new Course { Department = "Cybernetics", Number = 4385, Name = "Sentient AI Ethics and Governance", Professor = professorThree},
    new Course { Department = "Cybernetics", Number = 6490, Name = "Temporal AI: Predictive Systems and Time Manipulation", Professor = professorThree},
};

List<Student> listofStudents = new List<Student>() {
    new Student { FirstName = "Luke", LastName = "Skywalker" },
    new Student { FirstName = "Leia", LastName = "Organa" },
    new Student { FirstName = "Han", LastName = "Solo"},
    new Student { FirstName = "Obi-Wan", LastName = "Kenobi"},
};

List<StudentCourse> listOfEnrollments = new List<StudentCourse>() {
    new StudentCourse { Student = listofStudents[0], Course = listOfCourses[0] },
    new StudentCourse { Student = listofStudents[0], Course = listOfCourses[1] },
    new StudentCourse { Student = listofStudents[0], Course = listOfCourses[2] },
    new StudentCourse { Student = listofStudents[1], Course = listOfCourses[0] },
    new StudentCourse { Student = listofStudents[1], Course = listOfCourses[1] },
    new StudentCourse { Student = listofStudents[1], Course = listOfCourses[2] },
    new StudentCourse { Student = listofStudents[2], Course = listOfCourses[1] },
    new StudentCourse { Student = listofStudents[2], Course = listOfCourses[2] },
    new StudentCourse { Student = listofStudents[3], Course = listOfCourses[0] },
};

db.Add(KareemDana);
db.AddRange(listOfCourses);
db.AddRange(listofStudents);
db.AddRange(listOfEnrollments);
db.Add(new Professor {FirstName = "Amjad", LastName = "Abdullat"});
db.SaveChanges();

// foreach (var course in db.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student))
// {
//     Console.WriteLine(course);
//     foreach (var sc in course.StudentCourses)
//     {
//         Console.WriteLine($"\t{sc.Student}");
//     }
// }

Student studentToChange = db.Students.Where(s => s.FirstName == "Luke").Single();
Course courseToChange = db.Courses.Where(c => c.Department == "CIDM").Where(c => c.Number == 2315).Single();
StudentCourse scToChange = db.StudentCourses.Find(studentToChange.StudentID, courseToChange.CourseID)!;
scToChange.Grade = 100;
db.SaveChanges();


studentToChange = db.Students.Where(s => s.FirstName == "Obi-Wan").Single();
courseToChange = db.Courses.Where(c => c.Department == "CIDM").Where(c => c.Number == 2315).Single();
StudentCourse scToRemove = db.StudentCourses.Find(studentToChange.StudentID, courseToChange.CourseID)!;
db.Remove(scToRemove);
db.SaveChanges();
Course addCourse = db.Courses.Where(c => c.Department == "CIDM").Where(c => c.Number == 3312).Single();
StudentCourse newStudentCourse = new StudentCourse { Student = studentToChange, Course = addCourse};
db.Add(newStudentCourse);
db.SaveChanges();

foreach (var student in db.Students.Include(s => s.StudentCourses).ThenInclude(sc => sc.Course))
{
    Console.WriteLine(student);
    foreach (var sc in student.StudentCourses)
    {
        Console.WriteLine($"\t{sc.Course} - Grade: {sc.Grade}");
    }
}

Professor existingProf = db.Professors.Where(p => p.FirstName == "Amjad").Single();
Course newCourse = new Course { Department = "Business", Number = 4360, Name = "Maximizing Profits through Exploitation" };

// Three ways to connect a new course to an existing professor
// You should only use ONE of the ways.
// Usually technique 1 is most convenient, especially in ASP.NET Core apps
// where you will often have access to the ID
// (1) Add the Primary Key of the Professor (ProfessorID) as the Foreign Key to the Course class
newCourse.ProfessorID = existingProf.ProfessorID;
// (2) Add the entire professor Object as the navigation property to the Course Class
// newCourse.Professor = existingProf;
// (3) Add the entire new course to the list of courses (navigation property) in the Professor class
// existingProf.Courses = new List<Course> { newCourse };

// Finally add the course to the database and save changes
db.Add(newCourse);
db.SaveChanges();

// Demonstrate Foreign Key Constraint Failed
//db.Add(new Course { Department = "Cybernetics", Number = 4350, Name = "Synthetic Domination: Mastering Cybernetic Control Systems" });
//db.SaveChanges();

// (1) Find the course you want to update using either .Where() or .Find() if you know the Primary Key
// I used .Where() and .Single() to get a single result.
Course courseToModify = db.Courses.Where(c => c.Name == "Database Systems Design").Single();

// (2) Find the new professor who will be teaching that course.
Professor newProfessor = db.Professors.Where(p => p.FirstName == "Sean").Where(p => p.LastName == "Humpherys").Single();

// (3) Change the foreign key of the course.
// You can also update either navigation property.
courseToModify.ProfessorID = newProfessor.ProfessorID;

// (4) Save changes back to the database.
db.SaveChanges();

// (1) Find the professor you want to delete using either .Where() or .Find()
Professor profToDelete = db.Professors.Where(p => p.FirstName == "Amjad").Where(p => p.LastName == "Abdullat").Single();
// (2) Remove the professor. All courses taught by this professor will be deleted as well
db.Remove(profToDelete);
// (3) Save changes
db.SaveChanges();

// (1) Find the course you want to delete
Course courseToDelete = db.Courses.Where(c => c.Name == "Advanced Business Programming").Single();

// (2) Remove the course. The professor will remain teaching other courses.
db.Remove(courseToDelete);

// (3) Save changes
db.SaveChanges();

foreach (var course in db.Courses.Include(c => c.Professor))
{
    Console.WriteLine($"{course}. Taught by: {course.Professor}");
}

Console.WriteLine();
foreach (var p in db.Professors.Include(p => p.Courses))
{
    Console.WriteLine($"{p} teaches the following courses:");
    foreach (var c in p.Courses)
    {
        Console.WriteLine($"\t{c}");
    }
}

/*
Course firstCourse = db.Courses.First();
Console.WriteLine($"\n.First(): {firstCourse}");

Course? courseOrNull = db.Courses.FirstOrDefault();
if (courseOrNull == null)
{
    Console.WriteLine($"\n.FirstOrDefault(): NULL");
}
else
{
    Console.WriteLine($"\n.FirstOrDefault(): {courseOrNull}");
}

Course? courseTwenty = db.Courses.Find(20);
if (courseTwenty == null)
{
    Console.WriteLine($"\n.Find(20): NULL");
}

var query = db.Courses.Where(c => c.Department == "CIDM");
Console.WriteLine("\nCourses where Department = CIDM:");
foreach (var c in query)
{
    Console.WriteLine($"\t{c}");
}

var query2 = db.Courses.Where(c => c.Number >= 6000);

// I want to see cybernetics courses or courses with Quantum in their name
var query3 = db.Courses.Where(c => c.Department == "Cybernetics" || c.Name.Contains("Quantum"));
Console.WriteLine("\nCourses where Department = Cybernetics OR Quantum:");
foreach (var c in query3)
{
    Console.WriteLine($"\t{c}");
}
// I want to see cybernetics courses that contain AI in the name
var query4 = db.Courses.Where(c => c.Department == "Cybernetics" && c.Name.Contains("AI"));
Console.WriteLine("\nCourses in Cybernetics with AI:");
foreach (var c in query4)
{
    Console.WriteLine($"\t{c}");
}

var query5 = db.Courses.Where(c => c.Department == "Cybernetics").Where(c => c.Name.Contains("AI"));

var query6 = db.Courses.Where(c => c.CourseID == 1);
Course singleCourse = db.Courses.Where(c => c.CourseID == 1).Single();
Console.WriteLine($"\nSingle Course with ID of 1: {singleCourse}");

var query7 = db.Courses.OrderBy(c => c.Department);
Console.WriteLine("\nCourses ordered by department:");
foreach (var c in query7)
{
    Console.WriteLine($"\t{c}");
}

var query8 = db.Courses.OrderBy(c => c.Department).ThenBy(c => c.Number);
Console.WriteLine("\nCourses ordered by department then by course number:");
foreach (var c in query8)
{
    Console.WriteLine($"\t{c}");
}

var query9 = db.Courses.OrderByDescending(c => c.Department);
Console.WriteLine("\nCourses ordered in descending order:");
foreach (var c in query9)
{
    Console.WriteLine($"\t{c}");
}

var query10 = db.Courses.Where(c => c.Number >= 6000).OrderBy(c => c.Department);
Console.WriteLine("\nGraduate courses in order of department:");
foreach (var c in query10)
{
    Console.WriteLine($"\t{c}");
}
*/