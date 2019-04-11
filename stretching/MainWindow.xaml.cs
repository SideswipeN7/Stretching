using Stretching.App;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows.Controls;
using System.Windows.Media;

namespace Stretching
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StretchApp app_;
        private string[] Labels;

        public MainWindow()
        {
            InitializeComponent();
  
            app_ = new StretchApp(this);
            


            CartesianChart1.Series = new SeriesCollection
            {

                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(0,3),
                         new ObservablePoint(4,7),
                          new ObservablePoint(5,3),
                           new ObservablePoint(6,7),
                            new ObservablePoint(10,8)
                    },
                    PointGeometrySize = 5
                }
            };
       


        }




        private void Open_Click(object sender, RoutedEventArgs e)
        {
            app_.ReadFile();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            app_.SaveGraph();
        }


    }
}
