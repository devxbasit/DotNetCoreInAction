namespace EntityFrameworkCore.Domain.Common;

public abstract class BaseDomainEntity
{
    public int Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime LastUpdatedDateTime { get; set; }
    public int CreatedBy { get; set; }
    public int LastUpdatedBy { get; set; }
}
