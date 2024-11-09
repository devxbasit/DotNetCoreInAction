namespace TicketWebApi.Models;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedDateTime { get; set; } = DateTime.UtcNow;
}
