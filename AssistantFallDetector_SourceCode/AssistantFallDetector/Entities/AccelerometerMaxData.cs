using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantFallDetector.Entities
{
    public class AccelerometerMaxData
    {
        public AccelerometerMaxData()
        {
        }

        public double XAxisMax { get; set; }

        public double YAxisMax { get; set; }

        public double ZAxisMax { get; set; }

        public double AccelerationMax { get; set; }

    }
}
