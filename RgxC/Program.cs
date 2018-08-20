using LibSelection;
using RgxC.ASTranslator;
using RgxC.Translators;
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
            //Selection root = new Selection("hey{innerBlock{c=\"{{}{}{}}{}}}}}}}\";}}");
            //Selection def = root.Sel(0, 4);//hey{
            //def.Replace("newMethodName{");
            //Selection block = SimpleASTranslator2.GetToLevel(root, def, '{', '}', 0);
            //Console.WriteLine(block);

            Selection root = new Selection("buf already selected not selected");
            Selection selected = root.Sel(4, 16);
            var x = root.GetInverseSelections();
            Console.WriteLine(x);
            //new TranslatorDisplay<SimpleASTranslator2>().ShowDialog();
        }
    }
}
