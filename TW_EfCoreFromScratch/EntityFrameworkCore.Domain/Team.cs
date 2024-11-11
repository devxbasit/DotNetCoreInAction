using EntityFrameworkCore.Domain.Common;

namespace EntityFrameworkCore.Domain;

public class Team : BaseDomainEntity
{
    public string Name { get; set; }
    public int LeagueId { get; set; }
    public virtual League League { get; set; }
    public virtual List<Match> HomeMatches { get; set; }
    public virtual List<Match> AwayMatches { get; set; }
    public virtual Coach Coach { get; set; }
}
