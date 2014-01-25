using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantFallDetector.Entities
{
    public class GpsData
    {
        private DateTimeOffset timestamp;
        private double latitude;
        private double longitude;

        public GpsData()
        {
            Latitude = double.NaN;
            Longitude = double.NaN;
        }

        public GpsData(DateTimeOffset timestamp, double latitude, double longitude)
        {
            Timestamp = timestamp;
            Latitude = latitude;
            Longitude = longitude;
        }

        public DateTimeOffset Timestamp { get { return this.timestamp; } set { this.timestamp = value; } }

        public double Latitude { get { return this.latitude; } set { this.latitude = value; } }

        public double Longitude { get { return this.longitude; } set { this.longitude = value; } }

        public void SetGpsData(DateTimeOffset timestamp, double latitude, double longitude)
        {
            Timestamp = timestamp;
            Latitude = latitude;
            Longitude = longitude;
        }

        public GeoCoordinate GetGpsData()
        {
            GeoCoordinate coordinates = new GeoCoordinate(Latitude, Longitude);

            return coordinates;
        }

        public override string ToString()
        {
            return "Coordinates: \n" 
                + "Timestamp: " + timestamp.ToString() + "\n" 
                + "Latitude :" + latitude.ToString() + "\n"
                + "Longitude :" + longitude;
        }

    }
}
