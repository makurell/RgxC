using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibRgxC
{
    public class Selection
    {
        public string Value = string.Empty;
        public Selection Parent = null;
        public List<Selection> Children = new List<Selection>();

        internal int _off = 0;
        internal int _len = 0;

        public Selection(string value)
        {
            this.Value = value;
            this._len = value.Length;
        }

        public int GetAbs(int off=0)
        {
            int curoff = _off+off;
            Selection cursel = this;
            while (cursel.Parent != null)
            {
                curoff += cursel.Parent._off;
                cursel = cursel.Parent;
            }
            return curoff;
        }

        public List<Selection> GetAfter(int index)
        {
            List<Selection> ret = new List<Selection>();
            foreach(Selection child in Children)
            {
                if (child._off >= index)
                {
                    ret.Add(child);
                }
            }
            return ret;
        }

        public Selection Sel(int off, int len)
        {
            Selection ret = new Selection(Value.Substring(off, len));
            ret._off = off;
            ret.Parent = this;
            this.Children.Add(ret);
            return ret;
        }

        public void Replace(string repl)
        {
            //actual str manip
            Value = repl;

            int q = repl.Length - this._len;
            if(this.Parent != null)
            {
                foreach(Selection sel in this.Parent.GetAfter(this._off + this._len))
                {
                    sel._off += q;
                }
                string parentval = this.Parent.Value.Remove(this._off, this._len).Insert(this._off, repl);
                this.Parent.Replace(parentval);
            }
            this._len += q;
        }
    }
}
