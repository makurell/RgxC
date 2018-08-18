using LibSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgxC
{
    public abstract class Translator
    {
        public event Action<Selection> OnDebug = null;
        public Selection Root = null;
        public string Value { get { return Root.Value; } }

        public virtual Translator Setup(string input)
        {
            this.Root = new Selection(input);
            return this;
        }
        public abstract void Translate();
        public virtual void Debug(Selection curSelection)
        {
            OnDebug?.Invoke(curSelection);
        }
    }
}
