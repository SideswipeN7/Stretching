using System.Collections.Generic;

namespace Stretching.App.Data
{
    class StretchData
    {

        public string Path { get; set; }
        public double PreLoadValue { get; set; }
        public string PreLoadUnit { get; set; }
        public double TestSpeedValue { get; set; }
        public string TestSpeedUnit { get; set; }
        private IList<LineData> data_;

        /**
         * Methods that adds new line or multiple lines of data
         * @param LineData or LineData[]
         */
        public void Add(params LineData[] dataToAdd)
        {
            if (data_ == null)
            {
                data_ = new List<LineData>();
            }
            foreach (LineData line in dataToAdd)
            {
                data_.Add(line);

            }
        }

        /**
         * Methods that returns list of lines with data
         * @returns IList<LineData>
         */
        public IList<LineData> GetData() => data_;

        /**
         * Methods that returns line of data on correct id
         * @param id
         * @returns LineData
         */
        public LineData GetDataAt(int id) => data_[id];
    }
}
