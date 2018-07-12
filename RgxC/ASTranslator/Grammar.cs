using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgxC.ASTranslator
{
    public static class Grammar
    {
        //public const string s = @"\s*";
        public const string WS = @"(?:\s|\/\/.*?\n|\/[*].*?[*]\/)+";
        public const string IDENTIFIER = @"[$a-zA-Z_][a-zA-Z0-9_]*";

        public static string compilationUnit = b(packageDeclaration,
            e("{"), directives, compilationUnitDeclaration, e("}"));
        public static string packageDeclaration = b("package", o(qualifiedIde));
        private static string qualifiedIde = b(IDENTIFIER, r(b(e("."), IDENTIFIER)));
        private static string directives = r(directive);
        private static string directive = c(
            b("import",type,o(b(e("."),e("*")))),
            b(e("["),IDENTIFIER,c(e("("),annotationFields,e(")")),e("]")),
            b("use",IDENTIFIER,type),
            b(";"));
        private static string compilationUnitDeclaration = c(classDeclaration,memberDeclaration);

        #region builder methods
        public static string b(params string[] parts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for(int i = 0; i < parts.Length; i++)
            {
                sb.Append(parts[i]);
                if (i > parts.Length - 1)
                {
                    sb.Append(WS);
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        public static string c(params string[] parts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append("(");
                sb.Append(parts[i]);
                sb.Append(")");
                if (i > parts.Length - 1)
                {
                    sb.Append("|");
                }
            }
            sb.Append(")");
            sb.Append(WS);
            sb.Append(")");
            return sb.ToString();
        }

        public static string o(string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            sb.Append(part);
            sb.Append(")|)");
            return sb.ToString();
        }

        public static string r(string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            sb.Append(part);
            sb.Append(")*)");
            return sb.ToString();
        }

        public static string e(string s)
        {
            if (s == null || s.Length == 0)
            {
                return "";
            }

            char c = '\0';
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            String t;

            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                switch (c)
                {
                    case '\\':
                    case '"':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '/':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '.':
                        sb.Append("\\.");
                        break;
                    case '[':
                        sb.Append("\\[");
                        break;
                    case ']':
                        sb.Append("\\]");
                        break;
                    case '(':
                        sb.Append("\\(");
                        break;
                    case ')':
                        sb.Append("\\)");
                        break;
                    case '{':
                        sb.Append("\\{");
                        break;
                    case '}':
                        sb.Append("\\}");
                        break;
                    case '$':
                        sb.Append("\\$");
                        break;
                    case '^':
                        sb.Append("\\^");
                        break;
                    default:
                        if (c < ' ')
                        {
                            t = "000" + String.Format("X", c);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
