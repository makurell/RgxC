using LibRgxC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RgxC.Translators
{
    public class TestTranslator :Translator
    {
        public Regex rObject = new Regex(@"{([\s\S]+)}");
        public Selection Root = null;
        public TestTranslator(string text)
        {
            Root = new Selection(text);
        }
        public override Selection GetRoot()
        {
            return Root;
        }
        public override void Debug(Selection curSelection)
        {
            System.Threading.Thread.Sleep(1000);
            base.Debug(curSelection);
        }

        public override string Translate()
        {
            TranslateObject(Root);
            return "";
        }

        public void TranslateObject(Selection s)
        {
            Debug(s);
            s.Matches(rObject).ForEach(x => TranslateStatements(x.Group(1)));
        }

        public void TranslateStatements(Selection s)
        {
            Debug(s);
        }
    }
}
