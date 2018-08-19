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
        public const string IDENTIFIER_EXT = IDENTIFIER + "|" + @"[\<\>]*";
        public const string QUALIFIED_IDE = "((" + IDENTIFIER_EXT + ")" + "("+WS+"\\."+WS+"|)" + ")+";

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
        public static string r(string part, bool plus = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            sb.Append(part);
            sb.Append(")" + WS + ")");
            if (plus) sb.Append("+");
            else sb.Append("*");
            return sb.ToString();
        }

        /// <summary>
        /// wrap part in brackets
        /// </summary>
        public static string p(string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(part);
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// give part a name
        /// </summary>
        public static string n(string name, string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(?<" + name + ">" + part + ")");
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

        #region building helper methods
        /// <param name="plus">if true, cannot be empty</param>
        public static string upto(char target, bool plus = false)
        {
            return r(c(STRING_LITERAL, REGEXP_LITERAL, "[^" + e("" + target) + "]"), plus);
        }
        #endregion

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
        /// modifiers, constorvar, identifier, equals, expr, semicolon
        /// </summary>
        string VARIABLE_DECLARATION
        {
            get
            {
                return b(
                n("modifiers", r(MODIFIERS)),
                    n("constorvar", CONST_OR_VAR),
                    n("identifier", IDENTIFIER), o(TYPE_RELATION),
                    o(b(n("equals", "="), n("expr", upto(';')))),
                    n("semicolon", ";")
                );
            }
        }
        /// <summary>
        /// identifier, type
        /// </summary>
        string PARAMETER
        {
            get
            {
                return b(
                    o("const"),
                    n("identifier",IDENTIFIER),
                    o(TYPE_RELATION),
                    o(b("=",upto(',')))
                    );
            }
        }
        /// <summary>
        /// modifiers, identifier, parameters, type, block
        /// </summary>
        string METHOD_OR_CONSTRUCTOR_DECLARATION
        {
            get
            {
                return b(
                n("modifiers", r(MODIFIERS)),
                "function",
                o(c("get", "set")),
                n("identifier", IDENTIFIER),
                e("("),
                n("parameters",upto(')')),
                e(")"),
                o(TYPE_RELATION),
                n("block",
                    c(b(e("{"),upto('}'),e("}")),
                    ";"))
                );
            }
        }

        /// <summary>
        /// packageide
        /// </summary>
        string PACKAGE_DECLARATION
        {
            get
            {
                return b("package", n("packageide",o(QUALIFIED_IDE)));
            }
        }

        /// <summary>
        /// import
        /// </summary>
        string DIRECTIVE
        {
            get
            {
                //todo: annotationFields
                return c(
                    b(n("import","import"),TYPE,o(b(e("."),e("*")))),
                    b("use",IDENTIFIER,TYPE)
                    );
            }
        }
        /// <summary>
        /// modifiers, identifier, (extension)type, impls
        /// </summary>
        string CLASS_DECLARATION
        {
            get
            {
                return b(
                    n("modifiers",MODIFIERS),"class",n("identifier",IDENTIFIER),
                    o(b("extends",TYPE)),
                    o(b("implements",n("impls",upto('{'))))
                    );
            }
        }
        #endregion

        public override void Translate()
        {
            //TranslatePackage(Root);
            TranslateDirectives(Root);
            TranslateClassDeclaration(Root);
            TranslateVariables(Root);
            TranslateMethodOrConstructors(Root);
        }

        private void TranslateClassDeclaration(Selection sel)
        {
            foreach(RSelection classDeclaration in sel.Matches(CLASS_DECLARATION))
            {
                StringBuilder repl = new StringBuilder();
                repl.Append("${modifiers} class ${identifier}");
                StringBuilder inherits = new StringBuilder();
                if (classDeclaration.Group("type") != Selection.Empty)
                {
                    inherits.Append(classDeclaration.Group("type").Value.Trim());
                }
                if (classDeclaration.Group("impls") != Selection.Empty)
                {
                    if (!String.IsNullOrWhiteSpace(inherits.ToString()))
                    {
                        inherits.Append(",");
                    }
                    inherits.Append(classDeclaration.Group("impls").Value.Trim());
                }
                if (!String.IsNullOrWhiteSpace(inherits.ToString()))
                {
                    repl.Append(":"+inherits.ToString());
                }
                classDeclaration.Replace(repl.ToString());
            }
        }

        private void TranslateDirectives(Selection sel)
        {
            //todo more cases
            foreach(RSelection directive in sel.Matches(DIRECTIVE))
            {
                if (directive.Value.Trim().StartsWith("import"))
                {
                    directive.Replace(new Dictionary<string, string>()
                    {
                        {"import", "using"}
                    });
                }
            }
        }

        private void TranslatePackage(Selection sel)
        {
            foreach(RSelection package in sel.Matches(PACKAGE_DECLARATION))
            {
                package.Replace("namespace ${packageide}");
            }
        }

        private void TranslateMethodOrConstructors(Selection sel)
        {
            foreach(RSelection mocDeclaration in sel.Matches(METHOD_OR_CONSTRUCTOR_DECLARATION))
            {
                Debug(mocDeclaration);
                //translate parameters
                mocDeclaration.Replace(new Dictionary<string, ReplaceDelegate>
                {
                    {"parameters",(Selection input) =>
                    {
                        TranslateParameters(input);
                        return input.Value;
                    }},
                });
                //translate order
                mocDeclaration.Replace("${modifiers} ${type} ${identifier}(${parameters})${block}");
            }
        }

        private void TranslateParameters(Selection sel)
        {
            Debug(sel);
            foreach (RSelection parameter in sel.Matches(PARAMETER))
            {
                //word translation
                parameter.Replace(new Dictionary<string, ReplaceDelegate>
                {
                    {"type",(Selection input)=>
                    {
                        Debug(input);
                        return TranslateType(input.Value);
                    }},
                });
                //order translation
                parameter.Replace("${type} ${identifier}");
            }
        }

        //convert type to equivalent //todo
        public string TranslateType(string type)
        {
            switch (type.Trim())
            {
                case "String": return "string";
                case "Boolean": return "bool";
            }
            if (type.StartsWith("Vector.")) return type.Remove(6, 1);//remove the dot
            return type;
        }

        public void TranslateVariables(Selection sel)
        {
            foreach (RSelection fieldDeclaration in sel.Matches(VARIABLE_DECLARATION))
            {
                Debug(fieldDeclaration);
                //translate words
                fieldDeclaration.Replace(new Dictionary<string, ReplaceDelegate>
                {
                    {"type",(Selection input)=>
                    {
                        Debug(input);
                        return TranslateType(input.Value);
                    }},
                    {"modifiers",(Selection input)=>
                    {
                        Debug(input);
                        if (fieldDeclaration.Group("constorvar").Value.Trim() == "const")
                        {
                            return input.Value.Replace("static","");
                        }
                        return TranslateType(input.Value);
                    }},
                });
                //translate order
                fieldDeclaration.Replace("${modifiers} " + (fieldDeclaration.Group("constorvar").Value == "const" ? "${constorvar} " : "") + "${type} ${identifier}${equals}${expr}${semicolon}");
            }
        }
    }
}
