using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibRgxC.Exceptions
{
    public class ReplacementException : Exception
    {
        public ReplacementException(Exception innerException) : base("NB: Appending to end of selection is illegal", innerException)
        {
        }
    }
}
