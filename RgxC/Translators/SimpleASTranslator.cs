using LibSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RgxC.Translators
{
    public class SimpleASTranslator : ASTranslator
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
                    o(b(n("equals", "="), n("expr", upto(';',true)))),
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
                b("function",WSP),
                //o(b(c("get", "set"),WSP)),
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
        /// <summary>
        /// left,right
        /// </summary>
        string SIMPLE_ASSIGNMENT
        {
            get
            {
                return b(
                    n("left",upto('=',true,";")),"=",n("right",upto(';',true,"=")),";"
                    );
            }
        }
        #endregion

        public override void Translate()
        {
            //TranslatePackage(Root);
            TranslateDirectives(Root);
            TranslateClassDeclaration(Root);
            TranslateMethodOrConstructors(Root);
            TranslateVariables(Root);
            TranslateAssignments(Root);
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
                    {"type",(Selection input)=>
                    {
                        Debug(input);
                        return TranslateType(input.Value);
                    }},
                    {"block",(Selection input)=>
                    {
                        Debug(input);
                        TranslateBlock(input);
                        return input.Value;
                    }},
                });
                //translate order
                mocDeclaration.Replace("${modifiers} ${type} ${identifier}(${parameters})${block}\n");
            }
        }

        private void TranslateBlock(Selection sel)
        {
            //get inside the brackets
            Selection block = sel.Match(b("{",n("block",upto('}')),"}")).Group("block");
            foreach(RSelection statement in block.Matches(
                b(upto(';'),";")
                ))
            {
                TranslateVariables(statement);
                TranslateAssignments(statement);
            }
            sel.Replace("{\n" + block.Value + "}");
        }

        private void TranslateAssignments(Selection sel)
        {
            //simple assignment
            foreach(RSelection assignment in sel.Matches(SIMPLE_ASSIGNMENT))
            {
                assignment.Replace(new Dictionary<string, ReplaceDelegate>()
                {
                    {"right",(Selection right) =>
                    {
                        Debug(right);
                        TranslateExpr(right);
                        return right.Value;
                    }}
                });
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
                //case "String": return "string";
                //case "Boolean": return "bool";
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
                    {"expr",(Selection input)=>
                    {
                        Debug(input);
                        TranslateExpr(input);
                        return input.Value;
                    }},
                });
                //translate order
                fieldDeclaration.Replace("${modifiers} ${type} ${identifier}${equals}${expr}${semicolon}\n");
            }
        }

        private void TranslateExpr(Selection input)
        {
            string value = input.Value;
            //ltrim
            input.Match("^" + WS, RegexOptions.Multiline).Replace("");
            //rtrim
            input.Match(WS + "$", RegexOptions.Multiline).Replace("");

            //xml direct accessing -> indexer accessing
            foreach (RSelection xmlAccSel in input.Matches(b(
                @"xml([A-Z]\w+|)",n("parts",r(b(WS, e("."), WS, IDENTIFIER), true))
                )))
            {
                xmlAccSel.Replace(new Dictionary<string, ReplaceDelegate>()
                {
                    {"parts",(Selection parts)=>{
                        foreach(RSelection part in parts.Matches(b(WS, e("."), WS, n("identifier",IDENTIFIER))))
                        {
                            if(part.Group("identifier").Value.Trim()!="hasOwnProperty")
                            part.Replace("[\"${identifier}\"]");
                        }
                        return parts.Value;
                    }}
                });
            }

            //attribute operator
            foreach(RSelection attrSel in input.Matches(b(
                e("."),e("@"),n("identifier",IDENTIFIER)
                )))
            {
                attrSel.Replace(".attr(\"${identifier}\")");
            }

            //casting to parsing
            RSelection castSel = input.Match(b("^" + n("type", IDENTIFIER), n("rest", e("(") + ANY)) + "$");
            string castSelType = castSel.Group("type").Value;
            if(!string.IsNullOrWhiteSpace(castSelType))castSel.Replace("${rest}.to" + char.ToUpper(castSelType[0])+castSelType.Substring(1)+"()");
        }
    }
}
