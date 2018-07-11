using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibRgxC
{
    public class RSelection : Selection
    {
        //private GroupCollection _groups = null;
        private List<Selection> _groups = null;
        private string[] _groupnames = null;

        public RSelection(string value) : base(value)
        {
        }

        public RSelection(Regex regex, Match match) : base(match.Value)
        {
            this._off = match.Index;
            _groupnames = regex.GetGroupNames();
            _groups = new List<Selection>();
            foreach(Group g in match.Groups)
            {
                _groups.Add(Sel(g.Index - this._off, g.Length));
            }
        }

        public Selection Group(int groupnum)
        {
            //Group g = _groups[groupnum];
            //return Sel(g.Index-this._off, g.Length);
            return _groups[groupnum];
        }

        public Selection Group(string groupname)
        {
            //Group g = _groups[groupname];
            //return Sel(g.Index - this._off, g.Length);
            int i;
            for(i =0; i < _groupnames.Length; i++)
            {
                if (_groupnames[i] == groupname)
                {
                    break;
                }
            }
            return _groups[i];
        }

        /// <summary>
        /// can use $n, ${x}, $&, $$. Invalid sub will throw error
        /// </summary>
        public override void Replace(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '$')
                {
                    if (Char.IsDigit(s[i + 1]))
                    {
                        sb.Append(Group(Int32.Parse(""+s[i + 1])).Value);
                        i += 1;
                    }else if (s[i + 1] == '{')
                    {
                        string x = "";
                        for(i=i+2; i < s.Length; i++)
                        {
                            if (s[i] == '}') break;
                            x += s[i];
                        }
                        int groupnum;
                        if (Int32.TryParse(x, out groupnum))
                        {
                            sb.Append(Group(groupnum).Value);
                        }
                        else
                        {
                            sb.Append(Group(x).Value);
                        }
                    }else if (s[i + 1] == '&')
                    {
                        sb.Append(Group(0).Value);
                        i += 1;
                    }else if (s[i + 1] == '$')
                    {
                        sb.Append('$');
                        i += 1;
                    }
                }
                else
                {
                    sb.Append(s[i]);
                }
            }

            base.Replace(sb.ToString());
        }

        /// <summary>
        /// replace per group. No substitutions
        /// </summary>
        public void Replace(string[] replacements)
        {
            int i = 0;
            foreach(string repl in replacements)
            {
                if (repl != null)
                {
                    Group(i).Replace(repl);
                }
                i += 1;
            }
        }

        /// <summary>
        /// replace per group. No substitutions
        /// </summary>
        public void Replace(ReplaceDelegate[] dels)
        {
            int i = 0;
            foreach (ReplaceDelegate repl in dels)
            {
                if (repl != null)
                {
                    Group(i).Replace(repl);
                }
                i += 1;
            }
        }

        /// <summary>
        /// replace per group &lt;groupname,repl&gt;. No substitutions
        /// </summary>
        public void Replace(Dictionary<string,string> replacements)
        {
            foreach(KeyValuePair<string,string> pair in replacements)
            {
                Group(pair.Key).Replace(pair.Value);
            }
        }

        /// <summary>
        /// replace per group &lt;groupname,del&gt;. No substitutions
        /// </summary>
        public void Replace(Dictionary<string, ReplaceDelegate> replacements)
        {
            foreach (KeyValuePair<string, ReplaceDelegate> pair in replacements)
            {
                Group(pair.Key).Replace(pair.Value);
            }
        }
    }
}
