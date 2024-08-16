namespace EFCore2.Models;

public class Course
{
    public int CourseID {get; set;}
    public string Department {get; set;} = string.Empty;
    public int Number {get; set;}
    public string Name {get; set;} = string.Empty;
    public int ProfessorID {get; set;} // Foreign Key
    public Professor Professor {get; set;} = default!; // Navigation Property
    public List<StudentCourse> StudentCourses {get; set;} = default!; // Navigation property to StudentCourses

    public override string ToString()
    {
        return $"{Department} {Number} - {Name}";
    }
}