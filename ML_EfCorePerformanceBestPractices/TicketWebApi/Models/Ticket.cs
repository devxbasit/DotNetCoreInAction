namespace TicketWebApi.Models;

public class Ticket : BaseEntity
{
    public int EventId { get; set; }
    public double Price { get; set; }
}
