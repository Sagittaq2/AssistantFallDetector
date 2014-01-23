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
        public event AccelerometerReadData AccelerometerReadingChanged;

        Accelerometer accelerometer;
        //private const double G = 0.981;
        private const double G = 1;

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
                    ReportInterval = accelerometer.ReportInterval,
                    XAxis = Math.Round(args.Reading.AccelerationX, 3),
                    YAxis = Math.Round(args.Reading.AccelerationY, 3),
                    ZAxis = Math.Round(args.Reading.AccelerationZ, 3),
                    Acceleration = Math.Round((Math.Abs(Math.Sqrt(Math.Pow(args.Reading.AccelerationX,2) + Math.Pow(args.Reading.AccelerationY,2) + Math.Pow(args.Reading.AccelerationZ,2)))) - G, 3),                    
                };

                if (AccelerometerReadingChanged != null)
                    AccelerometerReadingChanged(data);

                //Mirar si se ha alcanzado el umbral y enviar notificación Raw

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
                ReportInterval = accelerometer.ReportInterval,
                XAxis = Math.Round(result.AccelerationX, 3),
                YAxis = Math.Round(result.AccelerationY ,3),
                ZAxis = Math.Round(result.AccelerationZ, 3),
                Acceleration = Math.Round((Math.Abs(Math.Sqrt(Math.Pow(result.AccelerationX, 2) + Math.Pow(result.AccelerationY, 2) + Math.Pow(result.AccelerationZ, 2)))) - G, 3)
            };
        }

    }
}
