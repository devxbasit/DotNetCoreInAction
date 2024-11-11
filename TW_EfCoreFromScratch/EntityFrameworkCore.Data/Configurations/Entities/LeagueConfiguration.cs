using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Data.Configurations.Entities;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    public void Configure(EntityTypeBuilder<League> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        // builder.HasData(
        //     new League()
        //     {
        //         Id = 1,
        //         Name = "Initial Seeded Data League 1",
        //     },
        //     new League()
        //     {
        //         Id = 2,
        //         Name = "Initial Seeded Data League 2",
        //     });
    }
}
