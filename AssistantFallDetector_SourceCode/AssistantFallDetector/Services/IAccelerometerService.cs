using AssistantFallDetector.Models;

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
