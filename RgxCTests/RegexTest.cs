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
            s.Matches(@"s(?<ac>\w*)d", RegexOptions.IgnoreCase)[0].Replace("$$1$1${1}1${ac}${aaa}$&$$${0}$0");
            Assert.AreEqual("$1TARTETARTE1TARTESTARTED$STARTEDSTARTED she said to slad the shed", s.Value);
        }

        [TestMethod]
        public void Test2()
        {
            Selection s = new Selection("{\n\"hello\":\"ylevol\"\n}");
            s.Match(@"{\s*""(?<key>\w+)""\s*:\s*""(?<value>\w+)""\s*}").Replace(null, "keyyay", "valyay");
            Assert.AreEqual("{\n\"keyyay\":\"valyay\"\n}", s.Value);

            s = new Selection("{\n\"hello\":\"ylevol\"\n}");
            s.Match(@"{\s*""(?<key>\w+)""\s*:\s*""(?<value>\w+)""\s*}").Replace(new Dictionary<string, string>()
            {
                {"value","valyay"},
                {"key","keyyay"}
            });
            Assert.AreEqual("{\n\"keyyay\":\"valyay\"\n}", s.Value);


            s = new Selection("{\n\"hello\":\"ylevol\"\n}");
            s.Match(@"{\s*""(?<key>\w+)""\s*:\s*""(?<value>\w+)""\s*}").Replace(
            null,
            (string key) =>
            {
                return key.ToUpper();
            },
            (string val) =>
            {
                return new string(val.ToCharArray().Reverse().ToArray());
            });
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
    }
}
