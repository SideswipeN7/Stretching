using System;

namespace Stretching.App.Parser
{
    class TraParseException : Exception
    {
        private readonly static string DEFAULT_TRA_PARSE_EXCEPTION = "Error on parsing TRA file";
        public TraParseException() : base(DEFAULT_TRA_PARSE_EXCEPTION) { }

        public TraParseException(string message) : base(message) { }
    }
}
