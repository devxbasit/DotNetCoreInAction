using System.Diagnostics;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

class Program
{
    private static readonly FootballLeagueDbContext _dbContext = new FootballLeagueDbContext();

    static async Task Main(string[] args)
    {
        await DeleteAllData();
        var league = await SimpleAddOneLeague("Summer League");
        await AddTeamsWithLeague(league);
        await AddChainedEntities();
        await PrintAllTeams();
        await QueryFilterExample();
        await SimpleUpdate();
        await SimpleDelete();
        await DeleteWithRelationships();
        await TrackingVsNoTracking();

    }

    static async Task TrackingVsNoTracking()
    {
        await Task.CompletedTask;
    }

    static async Task DeleteWithRelationships()
    {
        var league = await _dbContext.Teams.FirstAsync();
        _dbContext.Remove(league);
        await _dbContext.SaveChangesAsync();
    }

    static async Task SimpleDelete()
    {
        var team = await _dbContext.Teams.FirstAsync();
        _dbContext.Teams.Remove(team);
        await _dbContext.SaveChangesAsync();
    }

    static async Task SimpleUpdate()
    {
        var league = new League()
        {
            Name = "Test League Name for Update"
        };

        _dbContext.Leagues.Add(league);
        await _dbContext.SaveChangesAsync();

        league.Name = "Updated League Name";

        _dbContext.Leagues.Update(league);

        Console.WriteLine($"League name before Update : {league.Name}");
        await _dbContext.SaveChangesAsync();
        Console.WriteLine($"League name after Update : {league.Name}");
    }

    static async Task QueryFilterExample()
    {
        List<Team> teams;

        teams = await _dbContext.Teams.Where(x => x.Name.ToLower().Equals("team a")).ToListAsync();
        teams = await _dbContext.Teams.Where(x => x.Name.ToLower().Contains("e")).ToListAsync();
        teams = await _dbContext.Teams.Where(x => EF.Functions.Like(x.Name.ToLower(), "%e%")).ToListAsync();

        foreach (var team in teams)
        {
            Console.WriteLine($"Teams Name [Filter Example]: {team.Name}");
        }
    }

    static async Task PrintAllTeams()
    {
        var teamsDbSet = _dbContext.Teams;
        // extremely bad practice - below for loop will create a lock until all the collection is processed.
        // the connection will remain open, till the duration of for loop
        foreach (var team in teamsDbSet)
        {
            Console.WriteLine($"Team Name: {team.Name}");
        }

        var teams = _dbContext.Teams.ToList();
        // good practice - retrieve all at once, it will not create any lock
        foreach (var team in teams)
        {
            Console.WriteLine($"Team Name: {team.Name}");
        }
    }

    static async Task AddChainedEntities()
    {
        var league = new League() { Name = "Winter League" };
        var teams = new Team()
        {
            Name = "X Team in Winter League",
            League = league
        };


        // adding both in one shot
        await _dbContext.AddAsync(teams);
        await _dbContext.SaveChangesAsync();
    }

    static async Task AddTeamsWithLeague(League league)
    {
        var teams = new List<Team>()
        {
            new Team()
            {
                LeagueId = league.Id,
                Name = "Team A"
            },
            new Team()
            {
                LeagueId = league.Id,
                Name = "Team B"
            },
            new Team()
            {
                // setting navigation property directly
                League = league,
                Name = "Team C"
            }
        };

        await _dbContext.Teams.AddRangeAsync(teams);
        await _dbContext.SaveChangesAsync();
    }

    static async Task<League> SimpleAddOneLeague(string name)
    {
        League league = new()
        {
            Name = name
        };

        _dbContext.Leagues.Add(league);
        await _dbContext.SaveChangesAsync();
        return league;
    }

    static async Task DeleteAllData()
    {
        _dbContext.Teams.RemoveRange(_dbContext.Teams.ToList());
        _dbContext.Leagues.RemoveRange(_dbContext.Leagues.ToList());
        await _dbContext.SaveChangesAsync();
    }
}
