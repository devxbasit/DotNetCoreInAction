using System.Threading.Tasks;

namespace Polly.Interfaces
{
    public interface IAviationService
    {
        Task<string> GetFlights(string flightStatus);
    }
}