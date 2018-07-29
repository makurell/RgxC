using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSelection;

namespace RgxC.ASTranslator
{
    public class ASTranslator : Translator
    {
        Selection root = null;
        int index = 0;

        public ASTranslator(string raw)
        {
            root = new Selection(raw);
        }

        public override Selection GetRoot()
        {
            return this.root;
        }

        public override string Translate()
        {
            //if (!compilationUnit(root)) throw new ParsingException("Compilation Unit");
            return root.Value;
        }

        //public bool readLiteral(string literal)
        //{
        //    int index = 0;
        //    if(root.Value)
        //}

        //public bool compilationUnit(Selection sel)
        //{
        //    int start = index;
        //    if (!packageDeclaration()) throw new ParsingException("Package Declaration");
        //    if(!))

        //    sel.Sel(start, index);
        //}
    }
}
