using LibRgxC;
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
            System.Threading.Thread.Sleep(100);
            OnDebug?.Invoke(curSelection);
        }
    }
}
