using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LibSelection;
using RgxC.ASTranslator;

namespace RgxC.Translators
{
    public class GrammarTranslator : Translator
    {
        private static Regex rKey = new Regex(@"(?<key>\w+)\s*(?<equals>::=)");
        private static Regex rString = new Regex("\"(?<value>(\\\\\\\\|\\\\\"|[^\"])*)\"");
        private static Regex rBlock = new Regex(rKey.ToString()+ @"(?<block>((("+rString.ToString()+ @")|\w+)\s*|[^;])+)(?<semicolon>;)");

        public GrammarTranslator(string value) : base(value)
        {
        }

        public override void Debug(Selection curSelection)
        {
            base.Debug(curSelection);
        }

        public override void Translate()
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
                            return "public static string "+sel.Value+" "/*+@"""(?{"+sel.Value+@"}""+";*/+"{get{return ";
                        }},
                    {"equals", (Selection sel) =>
                        {
                            return "";
                            //return "= \"(?<"+block.Group("key").Value+">\"+";
                        }},
                    {"block", (Selection sel) =>
                        {
                            TranslateBlock(sel);
                            return sel.Value+/*"+\")\""*/";}}";
                        }},
                    {"semicolon", (Selection sel) =>
                        {
                            return "";
                        }},

                });
            }

            Debug(Root);
        }

        //private static Regex rChoice = new Regex("(?<choice>[^\\|]+)(\\||$)");
        private static Regex rChoice = new Regex("(?<choice>(((\"(?<value>(\\\\\\\\|\\\\\"|[^\"])*)\"|\\w+|\\[[^\\]]+\\]|\\([^\\)]+\\)|\\{[^\\}]+\\})\\s*)|\\|)+)");
        private static Regex rPart = new Regex("(\"(?<value>(\\\\\\\\|\\\\\"|[^\"])*)\"|\\w+|\\[[^\\]]+\\]|\\([^\\)]+\\)|\\{[^\\}]+\\}|\\|)");
        public void TranslateBlock(Selection sel)
        {
            var matches = sel.Matches(rChoice);
            //foreach (RSelection rsel in sel.Matches(rChoice))
            //{
            //    //rsel.Replace(new Dictionary<string, ReplaceDelegate>()
            //    //{
            //    //    {"choice", (Selection choice) =>
            //    //        {
            //    //            Debug(choice);
            //    //            return choice.Value;
            //    //        }}
            //    //});
            //    rsel.Replace(TranslateChoice(rsel.Group("choice")));
            //    rsel.Replace("b(" + rsel.Value + ")");
            //}
            for(int i = 0; i < matches.Count; i++)
            {
                RSelection rsel = matches[i];
                Debug(rsel);

                rsel.Replace(TranslateChoice(rsel.Group("choice"))/*+((i<matches.Count-1)?",":"")*/);
                //rsel.Replace("b(" + rsel.Value + ")");
                //rsel.Commit();
                //rsel.Matches("\\|").ForEach(x => x.Replace(""));
                //rsel.Replace(rsel.Value.Replace("|", ""));
            }
            sel.Replace("c(" + sel.Value + ")");
        }

        private string TranslateChoice(Selection selection)
        {
            Debug(selection);

            bool addSep = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("b(");

            var parts = selection.Matches(rPart);
            if (parts[parts.Count - 1].Value.Trim() == "|")
            {
                addSep = true;
                parts.RemoveAt(parts.Count - 1);
            }
            for (int i = 0; i < parts.Count; i++)
            {
                RSelection part = parts[i];
                Debug(part);
                string val = part.Value;

                if (val.StartsWith("\"") && val.EndsWith("\""))
                {
                    //string
                    sb.Append("e(\"");
                    sb.Append(Grammar.e1(val.Substring(1, val.Length - 2)));
                    sb.Append("\")");
                }
                else if (val.StartsWith("[") && val.EndsWith("]"))
                {
                    //optional
                    sb.Append("o(");
                    TranslateBlock(part.Sel(1, val.Length - 2));
                    sb.Append(part.Value.Substring(1, part.Value.Length - 2));
                    //sb.Append(TranslateChoice(part.Sel(1,val.Length-2)));
                    sb.Append(")");
                }
                else if (val.StartsWith("{") && val.EndsWith("}"))
                {
                    //repeated
                    sb.Append("r(");
                    TranslateBlock(part.Sel(1, val.Length - 2));
                    sb.Append(part.Value.Substring(1,part.Value.Length-2));
                    //sb.Append(TranslateChoice(part.Sel(1, val.Length - 2)));
                    sb.Append(")");
                }
                else
                {
                    //assuming identifier
                    sb.Append(val);
                }

                if (i < parts.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(")");
            if (addSep) sb.Append(",");
            return sb.ToString();//todo
        }

        //private string TranslatePart(RSelection part)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Debug(part);
        //    string val = part.Value;
        //    sb.Append("b(");

        //    if (val.StartsWith("\"") && val.EndsWith("\""))
        //    {
        //        //string

        //    }

        //    sb.Append(")");
        //    return sb.ToString();//todo
        //}
    }
}
