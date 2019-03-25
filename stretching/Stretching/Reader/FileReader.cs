using Microsoft.Win32;
using System;
using System.IO;

namespace Stretching.Reader
{
    class FileReader
    {
        private readonly static string DEFAULT_EXTENSIONS_FILTER = "All files (*.*)|*.*";
        public string extensionFilter { get; set; }
        private OpenFileDialog openFileDialog_;

        /**
         * Public constructor
         */
        public FileReader()
        {
            openFileDialog_ = new OpenFileDialog();
            extensionFilter = DEFAULT_EXTENSIONS_FILTER;
        }

        /**
         * Methods that read file content
         * @returns string[]
         */
        public string[] ReadFile()
        {
            string[] output = null;

            openFileDialog_.Multiselect = false;
            openFileDialog_.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            try
            {
                if (openFileDialog_.ShowDialog() == true)
                {
                    output = File.ReadAllText(openFileDialog_.FileName).Split('\n');
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in {nameof(FileReader)} method: {nameof(ReadFile)}" +
                    $"Error Type: {ex.GetType()}, Error message: {ex.Message}");
            }

            return output;
        }

    }
}
