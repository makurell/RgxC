using LibRgxC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RgxCTests
{
    [TestClass]
    public class RegexTest
    {
        [TestMethod]
        public void Test1()
        {
            Selection s = new Selection("she said to slad the shed");
            s.Replace((string input) => { return input.ToUpper() + "she said to slad the shed"; });
            s.Matches(@"s(?<ac>\w*)d", RegexOptions.IgnoreCase)[0].Replace("$$1$1${1}1${ac}$&$$${0}$0");//fixed overload ambiguity
            Assert.AreEqual("SHE $1AIAI1AISAID$SAIDSAID TO SLAD THE SHEDshe said to slad the shed", s.Value);
        }

        [TestMethod]
        public void Test2()
        {
            Selection s = new Selection("{\n\"hello\":\"ylevol\"\n}");
            s.Match(@"{\s*""(?<key>\w+)""\s*:\s*""(?<value>\w+)""\s*}").Replace(new string[] { null, "keyyay", "valyay" });
            Assert.AreEqual("{\n\"keyyay\":\"valyay\"\n}", s.Value);

            s = new Selection("{\n\"hello\":\"ylevol\"\n}");
            s.Match(@"{\s*""(?<key>\w+)""\s*:\s*""(?<value>\w+)""\s*}").Replace(new Dictionary<string, string>()
            {
                {"value","valyay"},
                {"key","keyyay"}
            });
            Assert.AreEqual("{\n\"keyyay\":\"valyay\"\n}", s.Value);


            s = new Selection("{\n\"hello\":\"ylevol\"\n}");
            s.Match(@"{\s*""(?<key>\w+)""\s*:\s*""(?<value>\w+)""\s*}").Replace(new ReplaceDelegate[] {
            null,
            (string key) =>
            {
                return key.ToUpper();
            },
            (string val) =>
            {
                return new string(val.ToCharArray().Reverse().ToArray());
            }});
            Assert.AreEqual("{\n\"HELLO\":\"lovely\"\n}", s.Value);

            s = new Selection("{\n\"hello\":\"ylevol\"\n}");
            s.Match(@"{\s*""(?<key>\w+)""\s*:\s*""(?<value>\w+)""\s*}").Replace(new Dictionary<string, ReplaceDelegate>()
            {
                {"value",(string val) =>
                    {
                        return new string(val.ToCharArray().Reverse().ToArray());
                    }},
                {"key",(string key) =>
                    {
                        return key.ToUpper();
                    }}
            });
            Assert.AreEqual("{\n\"HELLO\":\"lovely\"\n}", s.Value);
        }

        [TestMethod]
        public void Test3()
        {
            Regex r = new Regex(@"(((public|static)\s+)*)function\s+(\w+)");
            Selection s = new Selection("public static function findObjId");
            s.Match(r).Replace(new Dictionary<string, string>()
            {
                { "4", "methodName" },
                { "3","unstatic" },
            });
            s.Sel(0, 6).Replace("unpublic");
            Assert.AreEqual("unpublic unstatic function methodName",s.Value);
            s = new Selection("public static function findObjId");
            s.Match(r).Replace(new Dictionary<string, string>()
            {
                { "3","unstatic" },
                { "4", "methodName" },
            });
            s.Sel(0, 6).Replace("unpublic");
            Assert.AreEqual("unpublic unstatic function methodName", s.Value);
        }

        [TestMethod]
        public void BehaviorTest()
        {
            //changing the root after having children
            Regex r = new Regex(@"(((\w+)\s+)*)function\s+(\w+)");
            Selection s = new Selection("public static function findObjId");
            RSelection s2 = s.Match(r);
            s2.Replace(new Dictionary<string, string>()
            {
                { "4", "methodName" },
                { "3","unstatic" },
            });
            s.Sel(0, 6).Replace("unpublic");//changing root
            s2.Replace(new Dictionary<string, string>()
            {
                { "3","ee" },   //these pointers are now inaccurate+unpredictable because children are 'unlinked' from root
                { "4", "oo" },
            });
            Assert.AreEqual("public ee function oome", s.Value);//this result is unpredictable. This is what it produces atm (behavior)

            //this is what you should do instead:
            s = new Selection("public static function findObjId");
            s2 = s.Match(r);
            s2.Replace(new Dictionary<string, string>()
            {
                { "4", "methodName" },
                { "3","unstatic" },
            });
            s.Sel(0, 6).Replace("unpublic"); //after changing root...
            s.Commit(); //reset all pointers
            s2 = s.Match(r); //evaluate the pointers again
            s2.Replace(new Dictionary<string, string>()
            {
                { "3","ee" },
                { "4", "oo" },
            });
            Assert.AreEqual("unpublic ee function oo", s.Value);//this is what we would have expected

            //or like this:
            s = new Selection("public static function findObjId");
            s.Match(r).Replace(new Dictionary<string, string>()
            {
                { "4", "methodName" },
                { "3","unstatic" },
            });
            s.Sel(0, 6).Replace("unpublic");
            s.Match(r).Replace(new Dictionary<string, string>() //NB: pointers are being re-evaluated here
            {
                { "3","ee" },
                { "4", "oo" },
            });
            Assert.AreEqual("unpublic ee function oo", s.Value);
        }
    }
}
