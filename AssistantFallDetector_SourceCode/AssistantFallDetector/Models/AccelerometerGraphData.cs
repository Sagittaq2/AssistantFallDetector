
namespace AssistantFallDetector.Models
{
    public class AccelerometerGraphData
    {
        public AccelerometerGraphData()
        {
            this.XLineX1 = 240;
            this.XLineY1 = 150;
            this.XLineX2 = 340;
            this.XLineY2 = 150;

            this.YLineX1 = 240;
            this.YLineY1 = 150;
            this.YLineX2 = 240;
            this.YLineY2 = 50;

            this.ZLineX1 = 240;
            this.ZLineY1 = 150;
            this.ZLineX2 = 190;
            this.ZLineY2 = 200;

        }

        public double XLineX1 { get; set; }

        public double XLineY1 { get; set; }

        public double XLineX2 { get; set; }

        public double XLineY2 { get; set; }

        public double YLineX1 { get; set; }

        public double YLineY1 { get; set; }

        public double YLineX2 { get; set; }

        public double YLineY2 { get; set; }

        public double ZLineX1 { get; set; }

        public double ZLineY1 { get; set; }

        public double ZLineX2 { get; set; }

        public double ZLineY2 { get; set; }

    }
}
