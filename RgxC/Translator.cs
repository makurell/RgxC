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

        public abstract Selection GetRoot();
        public abstract string Translate();
        public virtual void Debug(Selection curSelection)
        {
            OnDebug?.Invoke(curSelection);
        }
    }
}
