using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgxC.Translators
{
    public abstract class ASTranslator:Translator
    {
        public const string IDENTIFIER = @"[$a-zA-Z_][a-zA-Z0-9_]*";
        public const string IDENTIFIER_EXT = IDENTIFIER + "|" + @"[\<\>]*";
        public const string QUALIFIED_IDE = "((" + IDENTIFIER_EXT + ")" + "(" + WS + "\\." + WS + "|)" + ")+";

        public const string STRING_LITERAL = @"""(\\\\|\\""|[^""])*""";
        public const string REGEXP_LITERAL = @"\/(\\\\|\\\/|[^\n])*\/[gisx]*";
        public const string INT_LITERAL = @"[0-9]+";
        public const string FLOAT_LITERAL = @"[0-9][0-9]*\.[0-9]+([eE][0-9]+)?[fd]?";
        public const string INFIX_OPERATOR = "=|\\*=|\\/=|%=|\\+=|\\-=|=|(>+)=|&=|\\^=|\\|=|\\|\\||&&|\\||\\^|&|==|!=|==(=+)|!==|(<+)|=|as|in|instanceof|is|>|(>+)|\\+|\\-|\\*|\\/|%";
        public const string PREFIX_OPERATOR = "\\+\\+|\\-\\-|\\+|\\-|!|~|typeof";
        public const string POSTFIX_OPERATOR = "\\+\\+|\\-\\-";

        public const string MODIFIERS = "public|protected|private|static|abstract|final|override|internal|";//can be empty
        public const string CONST_OR_VAR = "const|var";

        #region building helper methods
        /// <param name="plus">if true, cannot be empty</param>
        public static string upto(char target, bool plus = false, string additionalIllegal = "")
        {
            return r(c(STRING_LITERAL, REGEXP_LITERAL, "[^" + e("" + target) + additionalIllegal + "]"), plus);
        }
        #endregion

        public override abstract void Translate();

    }
}
