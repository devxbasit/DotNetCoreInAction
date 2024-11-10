using EntityFrameworkCore.Domain.Common;

namespace EntityFrameworkCore.Domain;

public class League : BaseDomainEntity
{
    public string Name { get; set; } = String.Empty;
    public List<Team> Teams { get; set; }
}
