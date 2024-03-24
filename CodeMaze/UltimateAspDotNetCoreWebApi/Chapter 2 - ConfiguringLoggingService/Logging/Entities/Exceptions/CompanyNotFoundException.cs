namespace Entities.Exceptions;

public class CompanyNotFoundException : NotFoundException
{
    public CompanyNotFoundException(Guid companyId)
        : base($"The Company with id : {companyId} does not exist in the datbase")
    {
    }
}