using Stretching.App;
using System.Windows;

namespace Stretching
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StretchApp app_;

        public MainWindow()
        {
            InitializeComponent();
            app_ = new StretchApp(this);
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            app_.DrawGraph();
        }

        private void BtnCalcR02_Click(object sender, RoutedEventArgs e)
        {
            app_.ToggleR02();
            app_.DrawGraph();
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            app_.ReadFile();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            app_.SaveGraph();
        }

    }
}
