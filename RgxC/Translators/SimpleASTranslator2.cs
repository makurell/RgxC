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
                o(TYPE_RELATION),
                "{"
                );
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
                    b(n("import", "import"), TYPE, o(b(e("."), e("*")))),
                    b("use", IDENTIFIER, TYPE)
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
                return b("package", n("packageide", o(QUALIFIED_IDE)),"{");
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
                    n("modifiers", MODIFIERS), "class", n("identifier", IDENTIFIER),
                    o(b("extends", TYPE)),
                    o(b("implements", n("impls", upto('{'))))
                    );
            }
        }
        #endregion

        #region parsing helper methods
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
                char c;
                try
                {
                    c = root.Value[startOffset + curOff];
                }
                catch(IndexOutOfRangeException ex)
                {
                    //EOF and not returned to targetLevel!
                    throw new ParsingException("EOF without reaching target level ("+targetLevel+") - Could not find corresponding closing bracket for bracket",root.GetLoc(startOffset),ex);
                }
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
        #endregion

        public override void Translate()
        {
            Debug(Root);
            TranslateComments(Root);//so that comments are not selected when InverseSelection
            TranslateMethods(Root);
            //now handle outside methods
            foreach (Selection outsideSel in Root.GetInverseSelections())
            {
                if (!String.IsNullOrEmpty(outsideSel.Value.Trim()))
                {
                    Console.WriteLine(outsideSel.Value);
                    TranslatePackageDeclarations(outsideSel);
                    TranslateDirectives(outsideSel);
                    TranslateClassDeclaration(outsideSel);
                }
            }
            Debug(Root);
        }

        private void TranslateComments(Selection root)
        {
            foreach (RSelection singleline in root.Matches(@"\/\/.*?\n"))
            {
                Debug(singleline);
            }
            foreach(RSelection multiline in root.Matches(@"\/\*(.|\s)*?\*\/"))
            {
                Debug(multiline);
            }
            //(just select them, don't do anything to them)
        }

        private void TranslateClassDeclaration(Selection sel)
        {
            foreach (RSelection classDeclaration in sel.Matches(CLASS_DECLARATION))
            {
                Debug(classDeclaration);
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
                    repl.Append(":" + inherits.ToString());
                }
                classDeclaration.Replace(repl.ToString());
            }
        }

        private void TranslateDirectives(Selection sel)
        {
            //todo more cases
            foreach (RSelection directive in sel.Matches(DIRECTIVE))
            {
                Debug(directive);
                if (directive.Value.Trim().StartsWith("import"))
                {
                    directive.Replace(new Dictionary<string, string>()
                    {
                        {"import", "using"}
                    });
                }
            }
        }

        private void TranslatePackageDeclarations(Selection root)
        {
            foreach (RSelection packageDeclaration in root.Matches(PACKAGE_DECLARATION))
            {
                Debug(packageDeclaration);
                packageDeclaration.Replace("namespace ${packageide}");
            }
        }

        public void TranslateMethods(Selection root)
        {
            foreach (RSelection methodStart in root.Matches(METHOD_OR_CONSTRUCTOR_START))
            {
                Debug(methodStart);
                //get block (sel everything until level 0 of brackets)
                Selection block = GetToLevel(root, methodStart, '{', '}');
                Debug(block);
                //consume ending bracket
                Selection endBracket = root.Sel(block.Off + block.Len, 1);
            }
        }
    }
}
