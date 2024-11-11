using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations.Entities;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder
            .HasMany(x => x.HomeMatches)
            .WithOne(x => x.HomeTeam)
            .HasForeignKey(x => x.HomeTeamId)
            .IsRequired();

        builder
            .HasMany(x => x.AwayMatches)
            .WithOne(x => x.AwayTeam)
            .HasForeignKey(x => x.AwayTeamId)
            .IsRequired();

    }
}
