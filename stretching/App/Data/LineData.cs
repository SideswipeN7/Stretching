namespace Stretching.App.Data
{
    class LineData
    {
        public double Strain { get; set; }
        public double StandardForce { get; set; }
        public double GripToGrip { get; set; }
        private double deltaL;

        public LiveCharts.Defaults.ObservablePoint ToObservablePoint() => new LiveCharts.Defaults.ObservablePoint() { X = deltaL, Y = StandardForce };

        internal void ReCalc(double? l0)
        {
            deltaL = (GripToGrip - (double)l0) / (double)l0;
        }
    }
}
