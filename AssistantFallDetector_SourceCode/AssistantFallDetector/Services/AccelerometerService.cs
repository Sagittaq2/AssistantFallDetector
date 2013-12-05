using AssistantFallDetector.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace AssistantFallDetector.Services
{
    public class AccelerometerService : IAccelerometerService
    {
        Accelerometer accelerometer;

        public void InitializeAccelerometer()
        {
            accelerometer = Accelerometer.GetDefault();
            accelerometer.ReportInterval = 33;
            if (accelerometer == null)
                throw new NotSupportedException("Accelerometer is not supported");

            accelerometer.ReadingChanged += accelerometer_ReadingChanged;
        }

        void accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            if (args.Reading != null)
            {
                AccelerometerData data = new AccelerometerData()
                {
                    XAxis = args.Reading.AccelerationX,
                    YAxis = args.Reading.AccelerationY,
                    ZAxis = args.Reading.AccelerationZ,
                    Acceleration = (Math.Abs(Math.Sqrt(Math.Pow(args.Reading.AccelerationX,2) + Math.Pow(args.Reading.AccelerationY,2) + Math.Pow(args.Reading.AccelerationZ,2))) - 1)
                };

                if (AccelerometerReadingChanged != null)
                    AccelerometerReadingChanged(data);
            }
        }

        public AccelerometerData GetAccelerometerRead()
        {
            if (accelerometer == null)
            {
                accelerometer = Accelerometer.GetDefault();
                if (accelerometer == null)
                    throw new NotSupportedException("Accelerometer is not supported");
            }

            var result = accelerometer.GetCurrentReading();

            return new AccelerometerData()
            {
                XAxis = result.AccelerationX,
                YAxis = result.AccelerationY,
                ZAxis = result.AccelerationZ,
                Acceleration = (Math.Abs(Math.Sqrt(Math.Pow(result.AccelerationX,2) + Math.Pow(result.AccelerationY,2) + Math.Pow(result.AccelerationZ,2))) - 1)
            };
        }

        public event AccelerometerReadData AccelerometerReadingChanged;
    }
}
