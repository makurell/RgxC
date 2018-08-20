using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgxC
{
    public class ParsingException : Exception
    {
        public ParsingException(string message) : base(message)
        {
        }

        public ParsingException(string message, Tuple<int, int, string> loc) : base(message + GetLocString(loc))
        {
        }

        public ParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ParsingException(string message, Tuple<int, int, string> loc, Exception innerException) : base(message+GetLocString(loc), innerException)
        {
        }

        public static string GetLocString(Tuple<int, int, string> loc)
        {
            return " - (" + loc.Item1 + ":" + loc.Item2 + ")\nPreview: " + loc.Item3;
        }
    }
}
