using DependencyInjection.Interfaces;

namespace DependencyInjection.Repositories
{
    public class TeamsRepository : ITeamsRepository
    {
        public string GetTeamName() {
            return "Test Team";
        }
    }
}