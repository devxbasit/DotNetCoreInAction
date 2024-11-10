namespace EfCore.ConsoleApp.Models;

public class Project
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime Deadline { get; set; }

    // many-to-many relationship - many employees with many project
    // one employee can be in more than one group
    // one project consists of many employees
    public ICollection<EmployeeProject> EmployeeProjects { get; set; } //collection navigation property to join/junction table
}
