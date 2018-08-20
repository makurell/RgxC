using LibSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RgxC
{
    public abstract class Translator
    {
        public const string WS = @"\s*";
        public const string WSP = @"\s+";
        public const string ANY = @"[\s\S]*";

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

        #region builder methods
        /// <summary>
        /// Separate parts with WS
        /// </summary>
        public static string b(params string[] parts)
        {
            if (parts.Length == 1) return parts[0];
            StringBuilder sb = new StringBuilder();
            //sb.Append("(");
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append(parts[i]);
                if (i < parts.Length - 1)
                {
                    sb.Append(WS);
                }
            }
            //sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Separate parts with or
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static string c(params string[] parts)
        {
            if (parts.Length == 1) return parts[0];
            StringBuilder sb = new StringBuilder();
            //sb.Append("((");
            for (int i = 0; i < parts.Length; i++)
            {
                //sb.Append("(");
                sb.Append(parts[i]);
                //sb.Append(")");
                if (i < parts.Length - 1)
                {
                    sb.Append("|");
                }
            }
            //sb.Append(")");
            //sb.Append(WS);
            //sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// make part optional
        /// </summary>
        public static string o(string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            sb.Append(part);
            sb.Append(")|)");
            return sb.ToString();
        }

        /// <summary>
        /// select part repeated
        /// </summary>
        public static string r(string part, bool plus = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("((");
            sb.Append(part);
            sb.Append(")" + WS + ")");
            if (plus) sb.Append("+");
            else sb.Append("*");
            return sb.ToString();
        }

        /// <summary>
        /// wrap part in brackets
        /// </summary>
        public static string p(string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append(part);
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// give part a name
        /// </summary>
        public static string n(string name, string part)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(?<" + name + ">" + part + ")");
            return sb.ToString();
        }

        /// <summary>
        /// regex escape
        /// </summary>
        public static string e(string s)
        {
            return Regex.Escape(s);
        }
        #endregion

        
    }
}
