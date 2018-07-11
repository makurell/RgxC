using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibRgxC
{
    public delegate string ReplaceDelegate(string input);

    public partial class Selection
    {
        public List<RSelection> Matches(string regex,RegexOptions options = RegexOptions.None)
        {
            return Matches(new Regex(regex,options));
        }

        public List<RSelection> Matches(Regex regex)
        {
            List<RSelection> ret = new List<RSelection>();
            foreach (Match m in regex.Matches(this.Value))
            {
                ret.Add(Match(regex,m));
            }
            return ret;
        }

        internal RSelection Match(Regex regex, Match match)
        {
            RSelection ret = new RSelection(regex,match);
            ret._off = match.Index;
            ret.Parent = this;
            this.Children.Add(ret);
            return ret;
        }

        public RSelection Match(Regex regex)
        {
            return Match(regex,regex.Match(this.Value));
        }

        public RSelection Match(string regex, RegexOptions options = RegexOptions.None)
        {
            return Match(new Regex(regex,options));
        }

        public void Replace(ReplaceDelegate del)
        {
            Replace(del?.Invoke(this.Value));
        }
    }
}
