using System.Threading.Tasks;

namespace HttpClient.Interfaces
{
    public interface IAviationService
    {
        Task<string> GetFlights(string flightStatus);
    }
}