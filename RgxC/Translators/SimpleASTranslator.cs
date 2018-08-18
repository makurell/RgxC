using LibSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RgxC.Translators
{
    public class SimpleASTranslator : Translator
    {
        public const string WS = @"\s*";
        public const string IDENTIFIER = @"[$a-zA-Z_][a-zA-Z0-9_]*";
        public const string QUALIFIED_IDE = IDENTIFIER+"("+WS+"\\."+WS+IDENTIFIER+"|)";

        public const string STRING_LITERAL = @"""(\\\\|\\""|[^""])*""";
        public const string REGEXP_LITERAL = @"\/(\\\\|\\\/|[^\n])*\/[gisx]*";
        public const string INT_LITERAL = @"[0-9]+";
        public const string FLOAT_LITERAL = @"[0-9][0-9]*\.[0-9]+([eE][0-9]+)?[fd]?";
        public const string INFIX_OPERATOR = "=|\\*=|\\/=|%=|\\+=|\\-=|=|(>+)=|&=|\\^=|\\|=|\\|\\||&&|\\||\\^|&|==|!=|==(=+)|!==|(<+)|=|as|in|instanceof|is|>|(>+)|\\+|\\-|\\*|\\/|%";
        public const string PREFIX_OPERATOR = "\\+\\+|\\-\\-|\\+|\\-|!|~|typeof";
        public const string POSTFIX_OPERATOR = "\\+\\+|\\-\\-";

        public const string MODIFIERS = "public|protected|private|static|abstract|final|override|internal";
        public const string CONST_OR_VAR = "const|var";

        #region builder methods
        /// <summary>
        /// Separate parts with WS
        /// </summary>
        public static string b(params string[] parts)
        {
            if (parts.Length == 1) return parts[0];
            StringBuilder sb = new StringBuilder();
            //sb.Append("(");
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append(parts[i]);
                if (i < parts.Length - 1)
                {
                    sb.Append(WS);
                }
            }
            //sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Separate parts with or
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static string c(params string[] parts)
        {
            if (parts.Length == 1) return parts[0];
            StringBuilder sb = new StringBuilder();
            //sb.Append("((");
            for (int i = 0; i < parts.Length; i++)
            {
                //sb.Append("(");
                sb.Append(parts[i]);
                //sb.Append(")");
                if (i < parts.Length - 1)
                {
                    sb.Append("|");
                }
            }
            //sb.Append(")");
            //sb.Append(WS);
            //sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// make part optional
        /// </summary>
        public static string o(string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            sb.Append(part);
            sb.Append(")|)");
            return sb.ToString();
        }

        /// <summary>
        /// select part repeated
        /// </summary>
        public static string r(string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            sb.Append(part);
            sb.Append(")" + WS + ")+");
            return sb.ToString();
        }

        /// <summary>
        /// give part a name
        /// </summary>
        public static string n(string name, string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(?<"+name+">" + part + ")");
            return sb.ToString();
        }

        /// <summary>
        /// regex escape
        /// </summary>
        public static string e(string s)
        {
            return Regex.Escape(s);
        }
        #endregion

        public override void Translate()
        {
            #region Variable Declarations
            string VARIABLE_DECLARATION = b(
                n("modifiers", r(MODIFIERS)),
                    n("constorvar", CONST_OR_VAR),
                    n("identifier",IDENTIFIER), o(b(":", n("type",c(e("*"),"void",QUALIFIED_IDE)))),
                    o(b(n("equals","="), n("expr",r(c(STRING_LITERAL, REGEXP_LITERAL, "[^;]"))), n("semicolon",";")))
                );
            foreach (RSelection fieldDeclaration in Root.Matches(VARIABLE_DECLARATION))
            {
                Debug(fieldDeclaration);
                //translate words
                fieldDeclaration.Replace(new Dictionary<string, ReplaceDelegate>
                {
                    //convert type to equivalent
                    {"type",(Selection input)=>
                    {
                        Debug(input);
                        switch (input.Value)
                        {
                            case "String": return "string";
                        }
                        return input.Value;
                    }},
                });
                //translate order
                fieldDeclaration.Replace("${modifiers} ${constorvar} ${type} ${identifier} ${equals} ${expr} ${semicolon}");
            }
            #endregion
        }
    }
}
