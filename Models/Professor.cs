namespace EFCore2.Models;

public class Professor
{
    public int ProfessorID {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    
    public List<Course> Courses {get; set;} = default!;

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}