namespace ExceptionalHandling.ExceptionalHandling.Domain.Exception
{
    public abstract class DomainException : System.Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}