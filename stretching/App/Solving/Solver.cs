using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using LiveCharts;
using Stretching.App.Data;
using LiveCharts.Defaults;

namespace Stretching.App.Solving
{
    class Solver
    {
        private LineSeries mainLineSeries;
        internal void PlotGraph(CartesianChart chart, IList<LineData> list)
        {
            var newList = list.Select(x => x.ToObservablePoint()).ToList();     //wybierz wszystkie elementy listy wykonaj na nich metode ToChartPoint i zapisz jako list
            mainLineSeries = new LineSeries() {Values = new ChartValues<ObservablePoint>(newList),  PointGeometrySize = 0 };

            chart.Series = new SeriesCollection
            {
                mainLineSeries
                          
            };

        }
    }
}
