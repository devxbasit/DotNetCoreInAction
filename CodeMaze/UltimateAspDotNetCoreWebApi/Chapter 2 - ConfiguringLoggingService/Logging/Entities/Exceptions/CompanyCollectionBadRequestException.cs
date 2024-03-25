namespace Entities.Exceptions;

public class CompanyCollectionBadRequestException : BadRequestException
{
    public CompanyCollectionBadRequestException() : base("Company collection send from a client is null.")
    {
    }
}