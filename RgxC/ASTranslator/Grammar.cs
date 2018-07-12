using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RgxC.ASTranslator
{
    public static class Grammar
    {
        //public const string s = @"\s*";
        public const string WS = @"(?:\s|\/\/.*?\n|\/[*].*?[*]\/)+";
        public const string IDENTIFIER = @"[$a-zA-Z_][a-zA-Z0-9_]*";

        public const string STRING_LITERAL = @"""(\\\\|\\""|[^""])*""";
        public const string REGEXP_LITERAL = @"/(\\\\|\\/|[^\n])*/[gisx]*";
        public const string INT_LITERAL = @"[0-9]+";
        public const string FLOAT_LITERAL = @"[0-9][0-9]*\.[0-9]+([eE][0-9]+)?[fd]?";
        public const string INFIX_OPERATOR = "=|\\*=|\\/=|%=|\\+=|\\-=|=|(>+)=|&=|\\^=|\\|=|\\|\\||&&|\\||\\^|&|==|!=|==(=+)|!==|(<+)|=|as|in|instanceof|is|>|(>+)|\\+|\\-|\\*|\\/|%";
        public const string PREFIX_OPERATOR = "\\+\\+|\\-\\-|\\+|\\-|!|~|typeof";
        public const string POSTFIX_OPERATOR = "\\+\\+|\\-\\-";


        #region builder methods
        public static string b(params string[] parts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < parts.Length; i++)
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
            return Regex.Escape(s);
        }

        public static string e1(string s)
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

        //Generated from GrammarTranslator.cs:
        #region generated
        public static string anonFunctionExpr = "(?<anonFunctionExpr>" + c(
            b(e("function"), e("("), parameters, e(")"), o(c(b(typeRelation))), block)) + ")";

        public static string annotationFields = "(?<annotationFields>" + c(
            b(o(c(b(annotationField, r(c(b(e(","), annotationField)))))))) + ")";

        public static string annotationField = "(?<annotationField>" + c(
            b(IDENTIFIER,"=", expr))+")";

        public static string arguments = "(?<arguments>" + c(
            b(o(c(b(exprOrObjectLiteral, r(c(b(e(","), exprOrObjectLiteral)))))))) + ")";

        public static string arrayLiteral = "(?<arrayLiteral>" + c(
            b(e("["), arguments, e("]"))) + ")";

        public static string block = "(?<block>" + c(
            b(e("{"), statements, e("}"))) + ")";

        public static string catches = "(?<catches>" + c(
            b(r(c(b(e("catch"), e("("), parameter, e(")"), block))))) + ")";

        public static string classBody = "(?<classBody>" + c(
            b(e("{"), r(c(b(directive), b(memberDeclaration), b(staticInitializer))), e("}"))) + ")";

        public static string classDeclaration = "(?<classDeclaration>" + c(
            b(modifiers, e("class"), IDENTIFIER, o(c(b(e("extends"), type))), o(c(b(e("implements"), type, r(c(b(e(","), type)))))), classBody)) + ")";

        public static string commaExpr = "(?<commaExpr>" + c(
            b(expr, r(c(b(e(","), expr))))) + ")";

        public static string compilationUnit = "(?<compilationUnit>" + c(
            b(packageDeclaration, e("{"), directives, compilationUnitDeclaration, e("}"))) + ")";

        public static string compilationUnitDeclaration = "(?<compilationUnitDeclaration>" + c(
            b(classDeclaration), b(memberDeclaration)) + ")";

        public static string constOrVar = "(?<constOrVar>" + c(
            b(e("const")), b(e("var"))) + ")";

        public static string directives = "(?<directives>" + c(
            b(r(c(b(directive))))) + ")";

        public static string directive = "(?<directive>" + c(
            b(e("import"), type, o(c(b(e("."), e("*"))))), b(e("["), IDENTIFIER, o(c(b(e("("), annotationFields, e(")")))), e("]")), b(e("use"), IDENTIFIER, type), b(e(";"))) + ")";

        public static string expr = "(?<expr>" + c(
            b(INT_LITERAL), b(FLOAT_LITERAL), b(STRING_LITERAL), b(REGEXP_LITERAL), b(e("true")), b(e("false")), b(e("null")), b(arrayLiteral), b(lvalue), b(anonFunctionExpr), b(e("this")), b(parenthesizedExpr), b(e("new"), type, o(c(b(e("("), arguments, e(")"))))), b(e("delete"), expr), b(PREFIX_OPERATOR, expr), b(expr, e("as"), type), b(expr, e("is"), expr), b(expr, POSTFIX_OPERATOR), b(expr, INFIX_OPERATOR, expr), b(expr, e("("), arguments, e(")")), b(expr, e("?"), exprOrObjectLiteral, e(":"), exprOrObjectLiteral)) + ")";

        public static string exprOrObjectLiteral = "(?<exprOrObjectLiteral>" + c(
            b(expr), b(objectLiteral), b(namedFunctionExpr)) + ")";

        public static string fieldDeclaration = "(?<fieldDeclaration>" + c(
            b(modifiers, constOrVar, identifierDeclaration, r(c(b(e(","), identifierDeclaration))))) + ")";

        public static string identifierDeclaration = "(?<identifierDeclaration>" + c(
            b(IDENTIFIER, o(c(b(typeRelation))), o(c(b(e("="), exprOrObjectLiteral))))) + ")";


        public static string labelableStatement = "(?<labelableStatement>" + c(
            b(e("if"), parenthesizedExpr, statement, e("else"), statement), b(e("if"), parenthesizedExpr, statement), b(e("switch"), parenthesizedExpr, e("{"), r(c(b(statementInSwitch))), e("}")), b(e("while"), parenthesizedExpr, statement), b(e("do"), statement, e("while"), parenthesizedExpr, e(";")), b(e("for"), e("("), o(c(b(commaExpr))), e(";"), o(c(b(commaExpr))), e(";"), o(c(b(commaExpr))), e(")"), statement), b(e("for"), e("("), e("var"), identifierDeclaration, r(c(b(e(","), identifierDeclaration))), e(";"), o(c(b(commaExpr))), e(";"), o(c(b(commaExpr))), e(")"), statement), b(e("for"), o(c(b(e("each")))), e("("), IDENTIFIER, e("in"), expr, e(")"), statement), b(e("for"), o(c(b(e("each")))), e("("), e("var"), IDENTIFIER, o(c(b(typeRelation))), e("in"), expr, e(")"), statement), b(e("try"), block, catches), b(e("try"), block, o(c(b(catches))), e("finally"), block), b(namedFunctionExpr), b(block)) + ")";

        public static string lvalue = "(?<lvalue>" + c(
            b(namespacedIdentifier), b(expr, e("."), namespacedIdentifier), b(expr, e("["), commaExpr, e("]")), b(e("super"), e("."), namespacedIdentifier)) + ")";

        public static string memberDeclaration = "(?<memberDeclaration>" + c(
            b(fieldDeclaration, e(";")), b(methodDeclaration)) + ")";

        public static string methodDeclaration = "(?<methodDeclaration>" + c(
            b(modifiers, e("function"), o(c(b(e("get")), b(e("set")))), IDENTIFIER, e("("), parameters, e(")"), o(c(b(typeRelation))), optBody)) + ")";

        public static string modifier = "(?<modifier>" + c(
            b(e("public")), b(e("protected")), b(e("private")), b(e("static")), b(e("abstract")), b(e("final")), b(e("override")), b(e("internal"))) + ")";

        public static string modifiers = "(?<modifiers>" + c(
           b(r(c(b(modifier))))) + ")";

        public static string namedFunctionExpr = "(?<namedFunctionExpr>" + c(
            b(e("function"), IDENTIFIER, e("("), parameters, e(")"), o(c(b(typeRelation))), block)) + ")";

        public static string namespacedIdentifier = "(?<namespacedIdentifier>" + c(
            b(o(c(b(modifier, e("::")))), IDENTIFIER)) + ")";

        public static string objectField = "(?<objectField>" + c(
            b(IDENTIFIER, e(":"), exprOrObjectLiteral), b(STRING_LITERAL, e(":"), exprOrObjectLiteral), b(INT_LITERAL, e(":"), exprOrObjectLiteral)) + ")";

        public static string objectFields = "(?<objectFields>" + c(
            b(o(c(b(objectField, r(c(b(e(","), objectField)))))))) + ")";

        public static string objectLiteral = "(?<objectLiteral>" + c(
            b(e("{"), objectFields, e("}"))) + ")";

        public static string optBody = "(?<optBody>" + c(
            b(block), b(e(";"))) + ")";

        public static string packageDeclaration = "(?<packageDeclaration>" + c(
            b(e("package"), o(c(b(qualifiedIde))))) + ")";

        public static string parameter = "(?<parameter>" + c(
            b(o(c(b(e("const")))), IDENTIFIER, o(c(b(typeRelation))), o(c(b(e("="), exprOrObjectLiteral))))) + ")";

        public static string parameters = "(?<parameters>" + c(
            b(o(c(b(parameter, r(c(b(e(","), parameter))))))), b(o(c(b(parameter, r(c(b(e(","), parameter))), e(",")))), IDENTIFIER, o(c(b(typeRelation))))) + ")";

        public static string parenthesizedExpr = "(?<parenthesizedExpr>" + c(
            b(e("("), exprOrObjectLiteral, e(")"))) + ")";

        public static string qualifiedIde = "(?<qualifiedIde>" + c(
            b(IDENTIFIER, r(c(b(e("."), IDENTIFIER))))) + ")";

        public static string statement = "(?<statement>" + c(
            b(e(";")), b(commaExpr, e(";")), b(IDENTIFIER, e(":"), labelableStatement), b(variableDeclaration, e(";")), b(e("break"), o(c(b(IDENTIFIER))), e(";")), b(e("continue"), o(c(b(IDENTIFIER))), e(";")), b(e("return"), o(c(b(exprOrObjectLiteral))), e(";")), b(e("throw"), commaExpr, e(";")), b(e("super"), e("("), arguments, e(")")), b(labelableStatement)) + ")";

        public static string statements = "(?<statements>" + c(b(r(c(b(statement))))) + ")";

        public static string statementInSwitch = "(?<statementInSwitch>" + c(
            b(statement), b(e("case"), expr, e(":")), b(e("default"), e(":"))) + ")";

        public static string staticInitializer = "(?<staticInitializer>" + c(
            b(block)) + ")";

        public static string type = "(?<type>" + c(
            b(qualifiedIde), b(e("*")), b(e("void"))) + ")";

        public static string typeList = "(?<typeList>" + c(
            b(type, r(c(b(e(","), typeList))))) + ")";

        public static string typeRelation = "(?<typeRelation>" + c(
            b(e(":"), type)) + ")";

        public static string variableDeclaration = "(?<variableDeclaration>" + c(
            b(constOrVar, identifierDeclaration, r(c(b(e(","), identifierDeclaration))))) + ")";
        #endregion
    }
}
