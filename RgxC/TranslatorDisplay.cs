using LibSelection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RgxC
{
    public partial class TranslatorDisplay<T> : Form where T : Translator, new()
    {
        public bool fastMode = false;
        public static string[] colours = new string[] { "indianred","hotpink","tomato","orchid","blueviolet","mediumslateblue","limegreen","mediumseagreen","steelblue","sandybrown"};
        Translator _translator = null;
        public TranslatorDisplay()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _translator = new T().Setup(textBox1.Text);
            _translator.OnDebug += _translator_OnDebug;
            new System.Threading.Thread(() => {
                _translator.Translate();
                StringBuilder sb = new StringBuilder();
                sb.Append("<!DOCTYPE html white-space:pre><html><head><script>function scroll(){document.getElementById(\"sel\").scrollIntoView(true);}</script><style>body{font-family: Consolas;white-space:PRE;font-size: 12px;}</style></head><body>");
                sb.Append(_translator.Value.Replace("\n","<br>"));
                sb.Append("</body></html>");
                this.Invoke(new Action(() =>
                {
                    webBrowser1.DocumentText = sb.ToString();
                }));
            }).Start();
            
        }
        public static int LastBefore(string str, int index, int n, string match)
        {
            if (n == 1)
            {
                return Math.Max(0, str.Substring(0, index).LastIndexOf(match));
            }
            else
            {
                return LastBefore(str,Math.Max(0, str.Substring(0, index).LastIndexOf(match)),n-1,match);
            }
        }
        public static void GetDescendants(ref List<Selection> list, int depth, Selection s)
        {
            if (depth == 0) return;
            foreach(Selection child in s.Children)
            {
                list.Add(child);
                GetDescendants(ref list, depth-1, child);
            }
        }
        private void _translator_OnDebug(Selection selection)
        {
            int selstart = selection.GetAbs();
            string total = _translator.Value;
            StringBuilder sb=null;
            if (!fastMode)
            {
                sb = new StringBuilder();

                sb.Append("<!DOCTYPE html white-space:pre><html><head><script>function scroll(){document.getElementById(\"sel\").scrollIntoView(true);}</script><style>body{font-family: Consolas;white-space:PRE;font-size: 12px;}</style></head><body>");

                int len = selection.Len;
                //total = _translator.GetRoot().Value;
                List<int> starts = new List<int>();
                List<int> ends = new List<int>();
                List<Selection> sels = new List<Selection>();
                GetDescendants(ref sels, 1, _translator.Root);
                foreach (Selection child in sels)
                {
                    int start = child.GetAbs();
                    starts.Add(start);
                    if (child.Len > 2)
                    {
                        ends.Add(start + child.Len - 1);
                    }
                }
                starts.Sort();
                starts.Reverse();

                for (int i = LastBefore(total, selstart, 5, "\n"); i < total.Length; i++)
                {
                    for (int j = 0; j < starts.Count; j++)
                    {
                        if (i == starts[j] && j != selstart)
                        {
                            sb.Append(@"<span id="""" style=""background-color:" + colours[j % colours.Length] + @""">");
                            break;
                        }
                    }
                    if (i == selstart)
                    {
                        sb.Append(@"<span id=""sel"" style=""background-color:lightgray"">");
                    }
                    char c = total[i];
                    switch (c)
                    {
                        case '\n':
                            sb.Append(@"<br>");
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                    if (i == selstart + len - 1)
                    {
                        sb.Append(@"</span>");
                    }
                    foreach (int end in ends)
                    {
                        if (i == end && i != selstart + len - 1)
                        {
                            sb.Append(@"</span>");
                            break;
                        }
                    }
                }

                sb.Append("</body></html>");
                System.Threading.Thread.Sleep(100);
            }

            if (/*selstart % 50 < 5 || */!fastMode)
            {
                this.Invoke(new Action(() =>
                {
                    if (!fastMode)
                    {
                        webBrowser1.DocumentText = sb.ToString();
                    }
                    progressBar1.Value = (int)(selstart / (double)(total.Length) * 10000);
                }));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_translator.Value);
        }
    }
}
