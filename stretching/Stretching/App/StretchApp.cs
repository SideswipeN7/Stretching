using Stretching.App.Data;
using Stretching.App.Notifications;
using Stretching.App.Parser;
using Stretching.Reader;
using System;
using static Stretching.App.Notifications.Notifier.MESSAGES;
using static Stretching.App.Notifications.Notifier.TITLES;
using static System.Windows.MessageBoxButton;
using static System.Windows.MessageBoxImage;

namespace Stretching.App
{
    class StretchApp
    {
        private readonly static string EXTENSIONS_FILTER = "Próba rozciągania (*.TRA)|*.TRA";
        private FileReader reader_;
        private TraParser parser_;
        private Notifier notifier_;
        private MainWindow window_;
        private StretchData data_;

        /******************************************************************************************/
        /******************************        Constructor       **********************************/
        /******************************************************************************************/

        /**
        * Public constructor
        */
        public StretchApp(MainWindow window) : base()
        {
            window_ = window;
        }

        /**
         * Private constructor
         */
        private StretchApp()
        {
            reader_ = new FileReader() { extensionFilter = EXTENSIONS_FILTER };
            parser_ = new TraParser();
            notifier_ = new Notifier();
        }


        /******************************************************************************************/
        /***************************        Public methods       **********************************/
        /******************************************************************************************/

        /**
         * Methods that reads file using FileReader
         */
        public void ReadFile()
        {
            var fileData = reader_.ReadFile();
            if (fileData != null)
            {
                try
                {
                    data_ = parser_.Parse(fileData);
                    OnDataLoad();
                }
                catch (TraParseException)
                {
                    //Show user that data in file are in correct
                    notifier_.Notify(WRONG_DATA, FILE_ERROR, OK, Error);
                }
            }
            else
            {
                //Show user that data there are no Data
                notifier_.Notify(NO_DATA_IN_FILE, FILE_ERROR, OK, Warning);
            }
        }


        /**
         * Methods that draws Line on graph
         */
        public void DrawLine()
        {
            //TODO: Draw line
            throw new NotImplementedException();
        }

        /**
         * Methods that draws R 0.2 Line on graph
         */
        public void DrawLineR02()
        {
            //TODO: Draw R02 line
            throw new NotImplementedException();
        }

        /**
         * Methods that saves graph as image
         */
        public void SaveGraph()
        {
            //TODO: Save plot as image
            throw new NotImplementedException();
        }

        /******************************************************************************************/
        /***************************       Private methods       **********************************/
        /******************************************************************************************/

        /**
         * Methods that is executed on data load from file
         */
        private void OnDataLoad()
        {
            if (data_ != null)
            {
                ShowData();
                PlotGraph();
            }
            else
            {
                //TODO: Show message cant plot graph
            }
        }

        /**
         * Methods that draws graph from data
         */
        private void PlotGraph()
        {
            //TODO: Plot graph
            throw new NotImplementedException();
        }

        /**
         * Methods that shows that from file
         */
        private void ShowData()
        {
            //TODO: Show data
            throw new NotImplementedException();
        }
    }
}
