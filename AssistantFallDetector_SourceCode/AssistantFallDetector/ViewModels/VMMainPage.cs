using AssistantFallDetector.Entities;
using AssistantFallDetector.Services;
using AssistantFallDetector.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AssistantFallDetector.ViewModels
{
    public class VMMainPage : VMBase
    {
        private IAccelerometerService accelerometerService;
        private IDispatcherService dispatcherService;
        private AccelerometerData data;
        private AccelerometerGraphData graphData;

        public VMMainPage(IAccelerometerService accelerometerService, IDispatcherService dispatcherService)
        {
            this.accelerometerService = accelerometerService;
            this.dispatcherService = dispatcherService;

            this.accelerometerService.AccelerometerReadingChanged += accelerometerService_AccelerometerReadingChanged;
            try
            {
                this.accelerometerService.InitializeAccelerometer();

                this.graphData = new AccelerometerGraphData();
                
            }
            catch (NotSupportedException exception)
            {
                this.accelerometerService.AccelerometerReadingChanged -= accelerometerService_AccelerometerReadingChanged;
                MessageBox.Show(exception.Message);
            }
        }

        void accelerometerService_AccelerometerReadingChanged(AccelerometerData data)
        {
            this.dispatcherService.CallDispatcher(() =>
            {
                Data = data;
                
                graphData.XLineX2 = graphData.XLineX1 + data.XAxis * 100;
                graphData.YLineY2 = graphData.YLineY1 - data.YAxis * 100;
                graphData.ZLineX2 = graphData.ZLineX1 - data.ZAxis * 50;
                graphData.ZLineY2 = graphData.ZLineY1 + data.ZAxis * 50;
                graphData.AccelerationLine = 200;// data.Acceleration * 1000;
                GraphData = graphData;
            });
        }

        public AccelerometerData Data
        {
            get { return this.data; }
            set
            {
                this.data = value;
                RaisePropertyChanged();
            }
        }

        public AccelerometerGraphData GraphData
        {
            get { return this.graphData; }
            set
            {
                this.graphData = value;
                RaisePropertyChanged();
            }
        }

    }
}
