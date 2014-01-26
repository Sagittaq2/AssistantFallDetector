
namespace AssistantFallDetector.Models
{
    public class AccelerometerData
    {
        public AccelerometerData()
        {
        }

        public double XAxis { get; set; }

        public double YAxis { get; set; }

        public double ZAxis { get; set; }

        public double Acceleration { get; set; }

        public uint ReportInterval { get; set; }

    }
}
