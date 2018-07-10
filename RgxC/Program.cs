using LibRgxC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgxC
{
    class Program
    {
        static void Main(string[] args)
        {
            Selection s = new Selection("hello this is a test");
            Console.WriteLine(s.Value);
            Selection w_this = s.Sel(6, 4);
            Selection w_a = s.Sel(14, 1);
            w_this.Replace("rahaha");
            Console.WriteLine(s.Value);
            Selection ha = w_this.Sel(2, 2);
            ha.Replace("hee");
            Console.WriteLine(s.Value);
            w_a.Replace("the");
            Console.WriteLine(s.Value);
        }
    }
}
