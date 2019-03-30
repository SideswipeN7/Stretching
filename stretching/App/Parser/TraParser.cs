using Stretching.App.Data;

namespace Stretching.App.Parser
{
    internal class TraParser
    {
        private static readonly string COMMENT_ROW_TEMPLATE = "$";
        private static readonly string PRELOAD_ROW_TEMPLATE = "Pre-load";
        private static readonly string TEST_SPEED_ROW_TEMPLATE = "Test speed";
        private static readonly string TITLE_ROW_TEMPLATE = "Strain;Standard force;Grip to grip s";
        private static readonly char[] SPLIT_ARRAY = { ' ' };
        private static readonly char SPLIT_LINE_DATA_SEPARATOR = ';';

        /**
         * Methods that Parse string lines to data
         * @param string[]
         * @return StretchData
         */
        public StretchData Parse(string[] data)
        {
            StretchData output = new StretchData();
            for (int i = 0; i < data.Length; ++i)
            {
                var row = data[i].Trim();
                if (!row.StartsWith(COMMENT_ROW_TEMPLATE) || !row.StartsWith(TITLE_ROW_TEMPLATE))
                {
                    if (row.StartsWith(PRELOAD_ROW_TEMPLATE))//Get PRE-LOAD
                    {
                        var preData = row.Split(SPLIT_ARRAY, 2);
                        if (double.TryParse(preData[1], out double preVal))
                        {
                            output.PreLoadValue = preVal;
                            output.PreLoadUnit = preData[2];
                        }
                        else
                        {
                            throw new TraParseException($"{PRELOAD_ROW_TEMPLATE} wrong value at row: {i + 1}");
                        }
                    }
                    else if (row.StartsWith(TEST_SPEED_ROW_TEMPLATE))//Get TEST SPEED
                    {
                        var speedData = row.Split(SPLIT_ARRAY, 3);
                        if (double.TryParse(speedData[2], out double speedVal))
                        {
                            output.TestSpeedValue = speedVal;
                            output.TestSpeedUnit = speedData[2];
                        }
                        else
                        {
                            throw new TraParseException($"{TEST_SPEED_ROW_TEMPLATE} wrong value at row: {i + 1}");
                        }
                    }
                    else//Get LINE DATA
                    {
                        LineData lineData = new LineData();
                        var values = row.Split(SPLIT_LINE_DATA_SEPARATOR);
                        if (double.TryParse(values[0], out double strainVal))
                        {
                            lineData.Strain = strainVal;
                        }
                        else
                        {
                            throw new TraParseException($"\"Strain\" data wrong value at row: {i + 1}");
                        }
                        if (double.TryParse(values[1], out double forceVal))
                        {
                            lineData.StandardForce = forceVal;
                        }
                        else
                        {
                            throw new TraParseException($"\"Standard force\" data wrong value at row: {i + 1}");
                        }
                        if (double.TryParse(values[2], out double gripVal))
                        {
                            lineData.GripToGrip = gripVal;
                        }
                        else
                        {
                            throw new TraParseException($"\"Grip to grip s\" data wrong value at row: {i + 1}");
                        }
                        output.Add(lineData);
                    }
                }
                else if (i == 0)//Get File Path
                {
                    output.Path = data[i];
                }
            }

            return output;
        }
    }
}