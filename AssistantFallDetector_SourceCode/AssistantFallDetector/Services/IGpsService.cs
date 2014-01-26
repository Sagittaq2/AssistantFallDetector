using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AssistantFallDetector.Services
{
    public delegate void PositionChangedArg(Geocoordinate coord);

    public interface IGpsService
    {
        Task<Geocoordinate> GetGpsCoordinates();
        Task<CivicAddress> GetGpsCivicAddress();

        event PositionChangedArg GpsPositionChanged;
    }
}
