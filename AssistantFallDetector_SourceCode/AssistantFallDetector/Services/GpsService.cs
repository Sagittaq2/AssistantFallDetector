using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace AssistantFallDetector.Services
{
    public class GpsService : IGpsService
    {
        public event PositionChangedArg GpsPositionChanged;

        Geolocator locator;

        /// <summary>
        /// Initialization of GPS sensor
        /// </summary>
        public GpsService()
        {
            locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;
            locator.MovementThreshold = 20;
            locator.PositionChanged += locator_PositionChanged;
        }

        /// <summary>
        /// GPS event when the position changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (GpsPositionChanged != null)
            {
                GpsPositionChanged(args.Position.Coordinate);
            }
        }

        /// <summary>
        /// Gets actual location of GPS in Geocoordinates
        /// </summary>
        /// <returns></returns>
        public async Task<Geocoordinate> GetGpsCoordinates()
        {
            var position = await locator.GetGeopositionAsync();

            return position.Coordinate;
        }

        /// <summary>
        /// Gets actual location of GPS in CivicAddress
        /// </summary>
        /// <returns></returns>
        public async Task<CivicAddress> GetGpsCivicAddress()
        {
            var position = await locator.GetGeopositionAsync();

            return position.CivicAddress;
        }
    }
}
