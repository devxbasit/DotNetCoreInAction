namespace EfCore.ConsoleApp.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public double Salary { get; set; }

    // 1 to 1 relationship with EmployeeDetails
    // Employee is principal/parent entity
    // EmployeeDetails is dependent/child entity
    public EmployeeDetails EmployeeDetails { get; set; } // reference navigation to dependent/child

    // 1 to many relationship - 1 manager to many employee
    // Manager is principal/parent entity
    // Employee is dependant/child entity
    public int ManagerId { get; set; } // required foreign key
    public Manager Manager { get; set; } // required reference navigation to principal/parent


    // many-to-many relationship - many employees with many project
    // one employee can be in more than one group
    // one project consists of many employees
    public ICollection<EmployeeProject> EmployeeProjects { get; set; } //collection navigation property to join/junction table
}
