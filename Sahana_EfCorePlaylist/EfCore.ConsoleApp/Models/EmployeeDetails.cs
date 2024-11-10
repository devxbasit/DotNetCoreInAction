namespace EfCore.ConsoleApp.Models;

public class EmployeeDetails
{
    public int EmployeeDetailsId { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }

    public int EmployeeId { get; set; } // required foreign key
    public Employee Employee { get; set; } // required reference navigation to principal/parent
}
