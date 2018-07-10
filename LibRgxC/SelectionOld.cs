using LibRgxC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibRgxC
{
    public class SelectionOld
    {
        public string Value = String.Empty;
        public SelectionOld Parent = null;

        private int _thisoff = 0;
        private int[] _offmap = null;

        public SelectionOld(string value)
        {
            this.Value = value;
            Init();
        }

        internal SelectionOld(string value, SelectionOld parent, int off)
        {
            this.Value = value;
            this.Parent = parent;
            this._thisoff = off;
            Init();
        }

        private void Init()
        {
            if (_offmap == null)
            {
                _offmap = new int[Value.Length];
                //_offmap = new int[Value.Length].ToList();
            }
        }

        //public int GetAbs(int off)
        //{
        //    int ret = _thisoff + this._offmap[off];
        //    Selection pselection = this.Parent;
        //    while (pselection != null)
        //    {
        //        ret -= pselection._offmap[ret] - pselection._thisoff;
        //        pselection = pselection.Parent;
        //    }
        //    return ret;
        //}

        internal void Replace(int off, int len, string repl)
        {
            int vlen = Value.Length;
            int mapoff = 0;
            try
            {
                mapoff = this._offmap[off];
            }
            catch (IndexOutOfRangeException ex)
            {
                //int neededno = (off - _offmap.Count);
                //for (int i = 0; i < neededno; i++)
                //{
                //    _offmap.Add(0);
                //}
                throw new ReplacementException(ex);
            }
            int curoff = mapoff + off;//was err here

            Value = Value.Remove(curoff, len);
            Value = Value.Insert(curoff, repl);

            int offincr = repl.Length - len;
            for(int i = curoff+1; i < _offmap.Length; i++)
            {
                _offmap[i] += offincr;
            }

            if(Parent != null)
            {
                Parent.Replace(_thisoff, vlen, Value);
            }
        }

        public void Replace(string repl)
        {
            int len = Value.Length;
            Value = repl;
            if(Parent != null)
            {
                Parent.Replace(_thisoff, len, Value);
            }
        }

        public SelectionOld Sel(int start, int count)
        {
            string selval = Value.Substring(start, count);
            int off = 0; //was err here
            try
            {
                off = this._offmap[start];
            }
            catch (IndexOutOfRangeException ex)
            {
                //int neededno = (start - _offmap.Count);
                //for (int i = 0; i < neededno; i++)
                //{
                //    _offmap.Add(0);
                //}
                throw new ReplacementException(ex);
            }
            return new SelectionOld(selval, this, start - off);
        }
    }
}
