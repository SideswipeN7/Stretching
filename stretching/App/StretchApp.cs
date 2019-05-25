using Stretching.App.Data;
using Stretching.App.Notifications;
using Stretching.App.Parser;
using Stretching.Reader;
using System;
using System.IO;
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
        private ComputedData computed_;
        private Logger.Logger logger_;
        private Solving.Solver solver_;
        private bool isR02_;

        /******************************************************************************************/
        /******************************        Constructor       **********************************/
        /******************************************************************************************/

        /**
        * Public constructor
        */
        public StretchApp(MainWindow window) : this()
        {
            window_ = window;
            //On start hide data grid
            ShowData();
            if (logger_.IsDebug)
            {
                //On start try to readFile
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir, @"..\..\data.TRA")));
                if (File.Exists(directory.ToString()))
                {
                    string[] fileData = File.ReadAllText(directory.ToString()).Split('\n');
                    OnFileRead(fileData);
                }
            }
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
         * Method that toggles visibility of R02
         */
        public void ToggleR02()
        {
            isR02_ = !isR02_;
            SetR02Visibility();
        }

        /**
         * Method that reads file using FileReader
         */
        public void ReadFile()
        {
            try
            {
                var fileData = reader_.ReadFile();
                isR02_ = false;
                OnFileRead(fileData);
            }
            catch (ArgumentException)//log file not selected
            {
                logger_.Log("File not selected", LOG_TYPE.WARNING);
            }
        }

        /**
         * Method that saves graph as image
         */
        public void SaveGraph()
        {
            solver_.SaveGraph(window_.CartesianChart1);
        }

        /**
         * Method that recalcs data and draws graphs
         */
        public void DrawGraph()
        {
            PlotGraph();
        }


        /******************************************************************************************/
        /***************************       Private methods       **********************************/
        /******************************************************************************************/

        /**
         * Method that is executed on data load from file
         */
        private void OnDataLoad()
        {
            double? l0, fi;
            try
            {
                l0 = GetL0();
                if (l0 == null)
                {
                    notifier_.Notify(L0_NAN, ERROR, OK, Error);
                }
                else
                {
                    try
                    {
                        fi = GetFi();
                        if (fi == null)
                        {
                            notifier_.Notify(FI_NAN, ERROR, OK, Error);
                        }
                        else
                        {
                            RecalcData(l0, fi);
                        }
                    }
                    catch (ArgumentException)
                    {
                        notifier_.Notify(NO_FI, ERROR, OK, Error);
                    }
                }
            }
            catch (ArgumentException)
            {
                notifier_.Notify(NO_L0, ERROR, OK, Error);
            }
            SetR02Visibility();
        }

        /**
         * Method to set visiblity of R02 data and change button message
         */
        private void SetR02Visibility()
        {
            if (isR02_)
            {
                window_.gridDataR02.Visibility = System.Windows.Visibility.Visible;
                window_.btnCalcR02.Content = "Ukryj R02";
            }
            else
            {
                window_.gridDataR02.Visibility = System.Windows.Visibility.Hidden;
                window_.btnCalcR02.Content = "Pokaż R02";
            }
        }

        /**
         * Method to recalculate data from
         */
        private void RecalcData(double? l0, double? fi)
        {
            if (dataVanilla_ != null)
            {
                data_ = dataVanilla_.Recalc(l0, fi);


                computed_ = solver_.CalculateValues(data_);
                PlotGraph();
                ShowData();
            }
            else
            {
                //TODO: Show message cant plot graph
            }
        }

        /**
         * Method that draws graph from data
         */
        private void PlotGraph()
        {
            solver_.PlotGraph(window_.CartesianChart1, isR02_);
        }

        /**
         * Method that shows that from file
         */
        private void ShowData()
        {
            if (data_ != null)
            {
                //show grid
                window_.GridComputed.Visibility = System.Windows.Visibility.Visible;
                //pre-load
                window_.lblPreLoadVal.Content = data_.PreLoadValue;
                window_.lblPreLoadUnit.Content = data_.PreLoadUnit;
                logger_.Log($"Read Pre-Load: {data_.PreLoadValue} {data_.PreLoadUnit}");
                //test speed
                window_.lblSpeedVal.Content = data_.TestSpeedValue;
                window_.lblSpeedUnit.Content = data_.TestSpeedUnit;
                logger_.Log($"Read Speed: {data_.TestSpeedValue} {data_.TestSpeedUnit}");
                //computed
                window_.lblDeltaLVal.Content = $"{computed_.DeltaL}";
                logger_.Log($"Computed Delta L: {computed_.DeltaL}");
                window_.lblRmVal.Content = $"{computed_.Rm}";
                window_.lblRmUnit.Content = $"{data_.PreLoadUnit}/mm2";
                logger_.Log($"Computed Rm: {computed_.Rm} {data_.PreLoadUnit}/mm2");
                window_.lblFmaxVal.Content = $"{computed_.Fmax}";
                window_.lblFmaxUnit.Content = $"{data_.PreLoadUnit}";
                logger_.Log($"Computed F max: {computed_.Fmax} {data_.PreLoadUnit}");
                window_.lblR02Val.Content = $"{computed_.R02}";
                window_.lblR02Unit.Content = $"{data_.PreLoadUnit}/mm2";
                logger_.Log($"Computed R02: {computed_.R02} {data_.PreLoadUnit}/mm2");
            }
            else
            {
                //hide grid
                window_.GridComputed.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        /**
         * Method to be executed when file data are retrieved
         * @param fileData - data of file in string array
         */
        private void OnFileRead(string[] fileData)
        {
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

        /**
         * Method to get value of L0 dimension from window
         * @returns double? - value if paraseble or null if not parsable
         * @throws ArgumentException if textbox value if empty
         */
        private double? GetL0()
        {
            if (window_.txbL0.Text == string.Empty)
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
        /**
         * Method to get value of Φ dimension from window
         * @returns double? - value if paraseble or null if not parsable
         * @throws ArgumentException if textbox value if empty
         */
        private double? GetFi()
        {
            if (window_.txbfi.Text == string.Empty)
            {
                //Log
                logger_.Log("Fi is empty", LOG_TYPE.ERROR);
                throw new ArgumentException();
            }

            if (double.TryParse(window_.txbfi.Text, out double result))
            {
                //Log
                logger_.Log($"Fi = {result} mm", LOG_TYPE.INFO);
                return result;
            }
            //Log
            logger_.Log($"Fi is NAN = {window_.txbfi.Text}", LOG_TYPE.ERROR);
            return null;
        }

    }
}
