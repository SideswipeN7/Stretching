namespace Stretching.App.Data
{
    class LineData
    {
        public double Strain { get; set; }
        public double StandardForce { get; set; }
        public double GripToGrip { get; set; }

        public LiveCharts.Defaults.ObservablePoint ToObservablePoint() => new LiveCharts.Defaults.ObservablePoint() { X = Strain, Y = StandardForce };
    }
}
