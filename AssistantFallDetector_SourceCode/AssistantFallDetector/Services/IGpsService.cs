using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace AssistantFallDetector.Services
{
    public delegate void PositionChangedArg(Geocoordinate coord);

    public interface IGpsService
    {
        event PositionChangedArg GpsPositionChanged;

        Task<Geocoordinate> GetGpsCoordinates();
        Task<CivicAddress> GetGpsCivicAddress();
    }
}
