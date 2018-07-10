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
            Console.WriteLine("Test 1:");
            Selection s = new Selection("hello this is a test");
            Selection w_hello = s.Sel(0, 5);
            Selection w_this = s.Sel(6, 4);
            Selection w_is = s.Sel(11, 2);
            w_this.Replace("banana");
            Console.WriteLine(s.Value);//hello banana is a test
            w_this.Replace("r");
            Console.WriteLine(s.Value);//hello r is a test
            w_is.Replace("shall be");
            Console.WriteLine(s.Value);//hello r shall be a test
            Selection test = s.Sel(19, 4);
            test.Replace("cookie");
            Console.WriteLine(s.Value);//hello r shall be a cookie
            test.Replace("cookies + food");
            Console.WriteLine(s.Value);//hello r shall be a cookies + food
            w_hello.Replace("bye");
            Console.WriteLine(s.Value);//bye r shall be a cookies + food

            s.Sel(5, 0).Replace("ee");
            Console.WriteLine(s.Value);//expect: bye ree shall be a cookies+food //FAIL --> byeee r shall be a cookies + food

            s.Sel(25, 0).Replace(" + meat");
            Console.WriteLine(s.Value);//(theoretically)hello r shall be a cookies + food + meat--> ReplacementException
        }
    }
}
