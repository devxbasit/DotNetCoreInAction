namespace EfCore.ConsoleApp.Models;

public class EmployeeProject
{
    // Join/Junction/bridging/linker table for many-to-many relationship
    // junction table contains two foreign keys
    public int EmployeeId { get; set; }
    public int ProjectId { get; set; }

    // reference navigation for both the foreign key tables
    public Employee Employee { get; set; }
    public Project Project { get; set; }
}
