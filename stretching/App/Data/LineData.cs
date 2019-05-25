namespace Stretching.App.Data
{
    class LineData
    {
        public double Strain { get; set; }
        public double StandardForce { get; set; }
        public double GripToGrip { get; set; }
        private double trueLength;

        /**
         * Method to change data in to ObservablePoint
         * @requies to Recalc be called first
         * @returns ObservablePoint
         */
        public LiveCharts.Defaults.ObservablePoint ToObservablePoint() => new LiveCharts.Defaults.ObservablePoint() { X = trueLength, Y = StandardForce };
        /**
         * Method to calculate value of true point length from l0
         * @param l0
         */
        internal void ReCalc(double? l0)
        {
            trueLength = GripToGrip + (double)l0;
        }
    }
}
