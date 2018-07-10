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
            Console.WriteLine("Test 1");
            Selection s = new Selection("hello this is a test");//hello this is a test
            Selection w_test = s.Sel(16, 4);
            Selection w_this = s.Sel(6, 4);
            Selection w_a = s.Sel(14, 1);
            w_this.Replace("rahaha");//hello rahaha is a test
            Selection ha = w_this.Sel(2, 2);
            ha.Replace("hee");//hello raheeha is a test
            w_a.Replace("the");//hello raheeha is the test
            w_test.Replace("cookie");//hello raheeha is the cookie
            Console.WriteLine(s.Value);
            //pass

        }
    }
}
