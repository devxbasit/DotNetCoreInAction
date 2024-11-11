using EntityFrameworkCore.Domain.Common;

namespace EntityFrameworkCore.Domain;

public class Match : BaseDomainEntity
{
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }

    public decimal TicketPrice { get; set; }

    public virtual Team HomeTeam { get; set; }
    public virtual Team AwayTeam { get; set; }

}
