namespace ExceptionalHandling.ExceptionalHandling.Domain.Exception
{
    public class DomainNotFoundException : DomainException
    {
        public DomainNotFoundException(string message) : base(message)
        {
        }
    }
}