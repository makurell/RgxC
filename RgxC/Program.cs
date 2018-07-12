using LibRgxC;
using RgxC.ASTranslator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RgxC
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(Grammar.labelableStatement);
            //new TranslatorDisplay().ShowDialog();
        }
    }
}
