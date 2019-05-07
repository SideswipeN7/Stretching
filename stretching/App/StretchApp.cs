using Stretching.App.Data;
using Stretching.App.Notifications;
using Stretching.App.Parser;
using Stretching.Reader;
using System;
using static Stretching.App.Notifications.Notifier.MESSAGES;
using static Stretching.App.Notifications.Notifier.TITLES;
using static Stretching.Logger.Logger;
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
        private StretchData dataVanilla_;
        private StretchData data_;
        private Logger.Logger logger_;
        private Solving.Solver solver_;

        /******************************************************************************************/
        /******************************        Constructor       **********************************/
        /******************************************************************************************/

        /**
        * Public constructor
        */
        public StretchApp(MainWindow window) : this()
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
            logger_ = new Logger.Logger();
            solver_ = new Solving.Solver();
            //Allow debug
            logger_.IsDebug = true;
            logger_.Log("APP Started");
        }


        /******************************************************************************************/
        /***************************        Public methods       **********************************/
        /******************************************************************************************/

        /**
         * Methods that reads file using FileReader
         */
        public void ReadFile()
        {
            try
            {
                var fileData = reader_.ReadFile();
                if (fileData != null)
                {
                    try
                    {
                        dataVanilla_ = parser_.Parse(fileData);
                        OnDataLoad();
                    }
                    catch (TraParseException ex)
                    {
                        //Show user that data in file are in correct
                        notifier_.Notify(WRONG_DATA, FILE_ERROR, OK, Error);
                        //Log
                        logger_.Log(ex.ToString(), LOG_TYPE.ERROR);
                    }
                }
                else
                {
                    //Show user that data there are no Data
                    notifier_.Notify(NO_DATA_IN_FILE, FILE_ERROR, OK, Warning);
                    //Log
                    logger_.Log("File with no data", LOG_TYPE.ERROR);
                }
            }
            catch (ArgumentException)//log file not selected
            {
                logger_.Log("File not selected", LOG_TYPE.WARNING);
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

        /**
         * Method that recalcs data and draws graphs
         */
        internal void DrawGraph()
        {
            OnDataLoad();
        }


        /******************************************************************************************/
        /***************************       Private methods       **********************************/
        /******************************************************************************************/

        /**
         * Methods that is executed on data load from file
         */
        private void OnDataLoad()
        {
            double? l0;
            try
            {
                l0 = GetL0();
                if (l0 == null)
                {
                    notifier_.Notify(L0_NAN, ERROR, OK, Error);
                }
                else
                {
                    RecalcData(l0);
                }
            }
            catch (ArgumentException ex)
            {
                notifier_.Notify(NO_L0, ERROR, OK, Error);
            }
        }

        /**
         * Method to recalculate data from
         */
        private void RecalcData(double? l0)
        {
            if (dataVanilla_ != null)
            {
                data_ = dataVanilla_.Recalc(l0);


                //ShowData();
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
            solver_.PlotGraph(window_.CartesianChart1, data_.GetData());
        }

        /**
         * Methods that shows that from file
         */
        private void ShowData()
        {
            //TODO: Show data
            throw new NotImplementedException();
        }

        /**
         * Method to get value of L0 dimension from window
         * @returns double? - value if paraseble or null if not parsable
         * @throws ArgumentException if textbox value if empty
         */
        private double? GetL0()
        {
            if (window_.txbL0.Text == String.Empty)
            {
                //Log
                logger_.Log("L0 is empty", LOG_TYPE.ERROR);
                throw new ArgumentException();
            }

            if (double.TryParse(window_.txbL0.Text, out double result))
            {
                //Log
                logger_.Log($"L0 = {result} mm", LOG_TYPE.INFO);
                return result;
            }
            //Log
            logger_.Log($"L0 is NAN = {window_.txbL0.Text}", LOG_TYPE.ERROR);
            return null;
        }
    }
}
