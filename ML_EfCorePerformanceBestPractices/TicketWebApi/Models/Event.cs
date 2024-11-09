namespace TicketWebApi.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Location { get; set; } = String.Empty;
    public List<Ticket> Tickets { get; set; }

}
