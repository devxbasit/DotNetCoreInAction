namespace EFCoreBasic.Models;

public class Passport: DomainBaseModel
{
    public string PassportNumber { get; set; }
    public string Country { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}
