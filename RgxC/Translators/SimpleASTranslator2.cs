using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibSelection;

namespace RgxC.Translators
{
    public class SimpleASTranslator2 : ASTranslator
    {
        #region regexes
        /// <summary>
        /// type
        /// </summary>
        string TYPE_RELATION
        {
            get { return b(":", TYPE); }
        }
        /// <summary>
        /// type
        /// </summary>
        string TYPE
        {
            get { return n("type", c(e("*"), "void", QUALIFIED_IDE)); }
        }
        /// <summary>
        /// modifiers, identifier, parameters, type, block
        /// </summary>
        string METHOD_OR_CONSTRUCTOR_START
        {
            get
            {
                return b(
                n("modifiers", r(MODIFIERS)),
                b("function", WSP),
                //o(b(c("get", "set"),WSP)),
                n("identifier", IDENTIFIER),
                e("("),
                n("parameters", upto(')')),
                e(")"),
                o(TYPE_RELATION)
                );
            }
        }
        #endregion

        /// <summary>
        /// does not include final closeBrac in selection. Do not include starting openBrac
        /// </summary>
        public static Selection GetToLevel(Selection root, Selection fromEndOf, char openBrac='{',char closeBrac='}', int targetLevel = 0)
        {
            return GetToLevel(root, fromEndOf.Off + fromEndOf.Len, openBrac, closeBrac, targetLevel);
        }

        /// <summary>
        /// does not include final closeBrac in selection. Do not include starting openBrac
        /// </summary>
        public static Selection GetToLevel(Selection root, int startOffset, char openBrac='{', char closeBrac='}', int targetLevel = 0)
        {
            //set initial level to 1
            int curLevel = 1;
            int curOff = 0;
            while (true)
            {
                string remaining = root.Value.Substring(startOffset + curOff);
                Match rgxMatch = Regex.Match(remaining, "^"+"("+STRING_LITERAL+"|"+REGEXP_LITERAL+")");
                if (rgxMatch.Success)
                {
                    curOff += rgxMatch.Length;
                }
                char c = root.Value[startOffset + curOff];
                if (c == openBrac)
                {
                    curLevel += 1;
                }
                else if (c == closeBrac)
                {
                    curLevel -= 1;
                }
                if (curLevel == targetLevel)
                {
                    break;
                }
                curOff += 1;
            }
            return root.Sel(startOffset, curOff);
        }

        public override void Translate()
        {
            TranslateMethods(Root);
        }

        public void TranslateMethods(Selection root)
        {
            //return list of excess selections
            foreach(RSelection methodStart in root.Matches(METHOD_OR_CONSTRUCTOR_START))
            {

            }
        }
    }
}
