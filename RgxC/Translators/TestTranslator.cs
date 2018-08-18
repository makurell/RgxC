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

        public override void Translate()
        {
            TranslateObject(Root);
            Debug(Root);
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
