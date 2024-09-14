namespace EFCoreBasic.Models;

public class Student: DomainBaseModel
{
    public string StudentName { get; set; }

    public Passport Passport { get; set; }
}
