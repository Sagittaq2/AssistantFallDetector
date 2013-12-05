using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantFallDetector.Entities
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

    }
}
