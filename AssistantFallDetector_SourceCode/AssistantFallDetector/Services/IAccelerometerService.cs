using AssistantFallDetector.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantFallDetector.Services
{
    public delegate void AccelerometerReadData(AccelerometerData data);

    public interface IAccelerometerService
    {
        void InitializeAccelerometer();

        AccelerometerData GetAccelerometerRead();

        event AccelerometerReadData AccelerometerReadingChanged;
    }
}
