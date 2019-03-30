using System;

namespace Stretching.Logger
{
    public class Logger
    {
        public enum LOG_TYPE { INFO, ERROR, WARNING };

        public bool IsDebug { get; set; } = false;

        /**
         * Method to log data to console
         * @param string message
         * @param LOG_TYPE
         */

        public void Log(string message, LOG_TYPE type = LOG_TYPE.INFO)
        {
            PrintText(MakeText(message, type), type);
        }

        /**
         * Method to log data to console in debug mode
         * @param string message
         * @param LOG_TYPE
         */

        public void Debug(string message, LOG_TYPE type = LOG_TYPE.INFO)
        {
            if (IsDebug)
            {
                Log(message, type);
            }
        }

        /**
         * Method to make text for logger
         * @param string message
         * @param LOG_TYPE type
         * @return string
         */

        private string MakeText(string message, LOG_TYPE type) => $"{DateTime.Now.ToUniversalTime()}  - {type.ToString()}: {message}";

        /**
         * Method to print text to correct console
         * @param string text
         * @param LOG_TYPE type
         */

        private void PrintText(string text, LOG_TYPE type)
        {
            if (type == LOG_TYPE.ERROR)
            {
                Console.Error.WriteLine(text);
            }
            else
            {
                Console.Out.WriteLine(text);
            }
        }
    }
}