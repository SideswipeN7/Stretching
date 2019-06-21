using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MathNet.Numerics;
using Microsoft.Win32;
using Stretching.App.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Stretching.App.Solving
{
    internal class Solver
    {
        private LineSeries mainLineSeries;
        private LineSeries otherLineSeries;
        private ObservablePoint f02;
        private static readonly string DATA_TITLE = "Wczytane dane";
        private static readonly string DATA_TITLE_LINE = "Umowna granica plastyczności";
        private static readonly string FILE_NAME = "Wykres";
        private static readonly string FILE_FILTER = "Pliki obrazów (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

        /**
         * Method to be plot graph
         * @param CartesianChart - chart element
         * @param bool - draw R02 or not
         */
        internal void PlotGraph(CartesianChart chart, bool drawR02)
        {
            //wybierz wszystkie elementy listy wykonaj na nich metode ToChartPoint i zapisz jako list

            if (drawR02)
            {
                chart.Series = new SeriesCollection{
                    mainLineSeries,
                    otherLineSeries
                };
            }
            else
            {
                chart.Series = new SeriesCollection{
                    mainLineSeries
                };
            }
        }

        /**
         * Method to Calculate points from file and R02
         * @param IList<LineData> - data from file
         */
        private void CalculatePoints(IList<LineData> data)
        {
            IList<double> pointsX = new List<double>();
            IList<double> pointsY = new List<double>();
            IList<ObservablePoint> oPoints = new List<ObservablePoint>();
            foreach (var point in data)
            {
                var calPoint = point.ToObservablePoint();
                pointsX.Add(calPoint.X);
                pointsY.Add(calPoint.Y);
                oPoints.Add(calPoint);
            }
            mainLineSeries = new LineSeries() { Values = new ChartValues<ObservablePoint>(oPoints), PointGeometry = null, Stroke = System.Windows.Media.Brushes.Red, Fill = System.Windows.Media.Brushes.Transparent, Title = DATA_TITLE };
            otherLineSeries = new LineSeries() { Values = new ChartValues<ObservablePoint>(GetNewPoints(pointsY, pointsX)), PointGeometry = null, Stroke = System.Windows.Media.Brushes.Blue, Fill = System.Windows.Media.Brushes.Transparent, Title = DATA_TITLE_LINE };
        }

        /**
         * Method to Calculate points from file and R02
         * @param IList<double> - data of Y values
         * @param IList<double> - data of X values
         * @return IList<ObservablePoint> - list of observable potins
         */
        private IList<ObservablePoint> GetNewPoints(IList<double> yPoints, IList<double> xPoints)
        {
            int maxNumber = (int)(0.2 * xPoints.Count);
            var item = Fit.Line(xPoints.Take(maxNumber).ToArray(), yPoints.Take(maxNumber).ToArray());
            double a = item.Item2;
            double b = item.Item1;
            IList<ObservablePoint> points = new List<ObservablePoint>();
            IList<ObservablePoint> pointsData = (IList<ObservablePoint>) mainLineSeries.Values;
            double moveRight = xPoints[0] * 1.002;
            double moveLeft = xPoints[0] * 0.998;
            bool rightOverLeft = moveRight > moveLeft;
            f02 = null;
            for (int i = 0; i < xPoints.Count; i++)
            {
                double x = xPoints[i] * 1.002;
                if (!rightOverLeft)
                {
                    x = xPoints[i] * 0.998;
                }
                var y = (xPoints[i] * a) + (b);

                var data = pointsData[i];
                var yLow = data.Y * 0.95;
                var yHigh = data.Y * 1.05;

                if (yLow>y && y < yHigh)
                {
                    f02 = data;
                }

                points.Add(new ObservablePoint(x, y));
            }

            return points;
        }

        /**
         * Method to Calculate data from file
         * @param StretchData - data from file
         * @return ComputedData - computed data for App
         */
        public ComputedData CalculateValues(StretchData data)
        {
            CalculatePoints(data.GetData());
            var fMax = data.GetData().Max(p => p.StandardForce);
            var dl = data.GetData().Last().GripToGrip;
            var s0 = Math.PI * Math.Pow((data.getFi() / 2.0), 2);
            var rm = fMax / s0;
            var r02 = CalculateR02(s0);
            return new ComputedData() { DeltaL = dl, Fmax = fMax, Rm = rm, R02 = r02, F02 = GetF02() };
        }

        /**
         * Method to Calculate R02
         * @param s0 - data to calculate
         * @return double - calculated R02
         */
        private double CalculateR02(double s0) => GetF02() / s0;

        /**
         * Method to Find F02
         * @return double F02 if null returns 0
         */
        private double GetF02()
        {
            if(f02 == null)
            {
                return 0;
            }
            else
            {
            return f02.Y;
            }
        }

        /**
         * Method to save Current Graph as image
         * @param CartesianChart - object to be saved as image
         */
        internal void SaveGraph(CartesianChart cartesianChart)
        {
            //Get renderer
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)cartesianChart.ActualWidth, (int)cartesianChart.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(cartesianChart);
            //Convert object
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            MemoryStream stream = new MemoryStream();
            png.Save(stream);
            Image image = Image.FromStream(stream);
            //Save object
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = FILE_NAME;
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                image.Save(saveFileDialog.FileName);
            }
        }
    }
}