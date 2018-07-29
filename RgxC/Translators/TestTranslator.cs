using LibSelection;
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
        public Regex rKey = new Regex(@"\""(.+)\""\s*:");

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
            base.Debug(curSelection);
        }

        public override string Translate()
        {
            TranslateObject(Root);
            Debug(Root);
            return Root.Value;
        }

        public void TranslateObject(Selection s)
        {
            Debug(s);
            s.Matches(rObject).ForEach(x => TranslateStatements(x.Group(1)));
        }

        public void TranslateStatements(Selection s)
        {
            Debug(s);
            s.Matches(rKey).ForEach(x =>
            {
                Debug(x);
                x.Replace("\"lol\":");
            });
        }
    }
}
