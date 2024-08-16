using System.ComponentModel.DataAnnotations;

namespace EFCore2.Models;

public class Student
{
    public int StudentID {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public List<StudentCourse> StudentCourses {get; set;} = default!; // Navigation property to StudentCourse

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }

}

public class StudentCourse
{
    public int StudentID {get; set;} // Primary Key, Foreign Key 1
    public int CourseID {get; set;} // Primary Key, Foreign Key 2
    
    [Range(0, 100)]
    public int Grade {get; set;}
    public Student Student {get; set;} = default!; // Navigation property back to Student
    public Course Course {get; set;} = default!; // Navigation property back to Course
}