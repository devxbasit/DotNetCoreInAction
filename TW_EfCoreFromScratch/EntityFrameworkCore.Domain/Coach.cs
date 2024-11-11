using EntityFrameworkCore.Domain.Common;

namespace EntityFrameworkCore.Domain;

public class Coach : BaseDomainEntity
{
    public string Name { get; set; }
    public int? TeamId { get; set; }
}
