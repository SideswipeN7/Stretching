using Stretching.App;
using System.Windows;
using System.Windows.Input;

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

        /******************************************************************************************/
        /**************************       Menu bar actions       **********************************/
        /******************************************************************************************/

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void AdjustWindowSize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaxButton.Tag = "1";
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaxButton.Tag = "2";
            }

        }
    }
}
