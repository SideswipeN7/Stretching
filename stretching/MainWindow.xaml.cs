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
