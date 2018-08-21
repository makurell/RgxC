using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSelection
{
    public partial class Selection
    {
        public static Selection Empty = new Selection(String.Empty);

        public string Value = string.Empty;
        public Selection Parent = null;
        public List<Selection> Children = new List<Selection>();

        internal int _off = 0;
        internal int _len = 0;

        public int Off { get { return _off; } }
        public int Len { get { return _len; } }

        public Selection(string value)
        {
            this.Value = value;//.Replace("\r\n", "\n").Replace("\r", "\n");
            this._len = value.Length;
        }

        public int GetAbs(int off = 0)
        {
            int curoff = _off + off;
            Selection cursel = this;
            while (cursel.Parent != null)
            {
                curoff += cursel.Parent._off;
                cursel = cursel.Parent;
            }
            return curoff;
        }

        public Selection GetRoot()
        {
            Selection cursel = this;
            while (cursel.Parent != null)
            {
                cursel = cursel.Parent;
            }
            return cursel;
        }

        public Tuple<int, int, string> GetLoc(int off = 0, int around = 30)
        {
            int curoff = _off + off;
            Selection cursel = this;
            while (cursel.Parent != null)
            {
                curoff += cursel.Parent._off;
                cursel = cursel.Parent;
            }

            string fullval = cursel.Value;
            string preview = (fullval.Substring(Math.Max(0, curoff - around), Math.Min(around, curoff)) + "@" + fullval.Substring(curoff, Math.Min(around, fullval.Length - curoff))).Replace("  ", "").Replace("\n", "");

            int curLineno = 1;
            int lasti = 0;
            int i = 0;
            for (i = 0; i < fullval.Length && i < curoff; i++)
            {
                if (fullval[i] == '\n')
                {
                    curLineno += 1;
                    lasti = i;
                }
            }
            return new Tuple<int, int, string>(curLineno, i - lasti, preview);
        }

        public void Commit()
        {
            Children = new List<Selection>();
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
            Selection ret = new Selection(Value.Substring(off, len))
            {
                _off = off,
                Parent = this
            };
            this.Children.Add(ret);
            return ret;
        }

        public virtual void Replace(string repl)
        {
            //actual str manip
            Value = repl;

            int q = repl.Length - this._len;
            if(this.Parent != null)
            {
                foreach(Selection sel in this.Parent.GetAfter(this._off + this._len))
                {
                    if (sel != this)
                    {
                        sel._off += q;
                    }
                }
                string parentval = this.Parent.Value.Remove(this._off, this._len).Insert(this._off, repl);
                this.Parent.Replace(parentval);
            }
            this._len += q;
        }

        public List<Selection> GetInverseSelections()
        {
            List<Selection> ret = new List<Selection>();
            List<Selection> remSels = new List<Selection>();
            foreach(Selection child in this.Children)
            {
                remSels.Add(child);
            }

            int prev = 0;
            int i = 0;
            while (true)
            {
                //check if i is in a sel, if is, then add len to i
                foreach(Selection remsel in remSels)
                {
                    if (i >= remsel.Off && i<remsel.Off+remsel.Len)
                    {
                        //push to ret
                        if (prev != i)
                        {
                            ret.Add(this.Sel(prev, i - prev));
                        }
                        //skip over this sel
                        i += remsel.Len;
                        prev = i;
                        remSels.Remove(remsel);
                        break;
                    }
                }
                if (i >= this.Len)
                {
                    //push to ret
                    if (prev != i)
                    {
                        ret.Add(this.Sel(prev, i - prev));
                    }
                    break;//break while loop
                }
                i += 1;
            }
            return ret;
        }
    }
}
