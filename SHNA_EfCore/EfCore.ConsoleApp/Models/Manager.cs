namespace EfCore.ConsoleApp.Models;

public class Manager
{
    public int ManagerId { get; set; }
    public string Name { get; set; }

    // 1 to many relationship - 1 manager to many employee
    // Manager is principal/parent entity
    // Employee is dependant/child entity

    public ICollection<Employee> Employees { get; set; } // collection navigation property to dependant/child
}
