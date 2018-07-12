using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibRgxC;

namespace RgxC.Translators
{
    public class GrammarTranslator : Translator
    {
        public static Regex rKey = new Regex(@"(?<key>\w+)\s*(?<equals>::=)");
        public static Regex rString = new Regex("\"(?<value>(\\\\\\\\|\\\\\"|[^\"])*)\"");
        public static Regex rBlock = new Regex(rKey.ToString()+ @"(?<block>((("+rString.ToString()+ @")|\w+)\s*|[^;])+);");
        public Selection Root = null;

        public GrammarTranslator(string value)
        {
            Root = new Selection(value);
        }

        public override Selection GetRoot()
        {
            return Root;
        }

        public override string Translate()
        {
            Debug(Root);

            //look at each block
            foreach(RSelection block in Root.Matches(rBlock))
            {
                Debug(block);
                block.Replace(new Dictionary<string, ReplaceDelegate>()
                {
                    {"key", (Selection sel) =>
                        {
                            return "public static string "+sel.Value;
                        }},
                    {"equals", (Selection sel) =>
                        {
                            return "=";
                        }},
                    {"block", (Selection sel) =>
                        {
                            return "BLOCK";
                        }},
                });
            }
            return Root.Value;
        }
    }
}
