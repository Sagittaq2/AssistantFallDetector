using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Devices.Geolocation;

namespace AssistantFallDetector.Services
{
    public class GpsService : IGpsService
    {
        public event PositionChangedArg GpsPositionChanged;

        Geolocator locator;

        public GpsService()
        {
            locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;
            locator.MovementThreshold = 20;
            locator.PositionChanged += locator_PositionChanged;
        }

        void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (GpsPositionChanged != null)
                GpsPositionChanged(args.Position.Coordinate);
        }

        public async Task<Geocoordinate> GetGpsCoordinates()
        {
            var position = await locator.GetGeopositionAsync();

            return position.Coordinate;
        }
    }
}
