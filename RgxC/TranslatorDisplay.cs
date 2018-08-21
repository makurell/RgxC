using LibSelection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace RgxC
{
    public partial class TranslatorDisplay<T> : Form where T : Translator, new()
    {
        public const string HTML_START = "<!DOCTYPE html white-space:pre><html><head><script>function scroll(){document.getElementById(\"sel\").scrollIntoView(true);}</script><style>body{font-family: Consolas;white-space:PRE;font-size: 12px;}</style></head><body>";
        public const string HTML_END = "</body></html>";
        public bool fastMode = false;
        public bool autoMode = true;
        public static string[] colours = new string[] {"hotpink","tomato","orchid","blueviolet","mediumslateblue","limegreen","mediumseagreen","steelblue","sandybrown"};
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
                string output = "";
                try
                {
                    _translator.Translate();
                    output = _translator.Value;
                }catch(Exception ex)
                {
                    output = ex.ToString();
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(HTML_START);
                sb.Append(output.Replace(@"&", @"&amp;").Replace(@"<", @"&lt;").Replace(@">", @"&gt;").Replace(@"'", @"&#39;").Replace(@"""", @"&quot;").Replace("\n", "<br>"));
                sb.Append(HTML_END);
                this.Invoke(new Action(() =>
                {
                    webBrowser1.DocumentText = sb.ToString();
                    if(this.fastMode)button2_Click(null, null);
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
        private string BuildVisualisation(Selection selection,bool wrap=true)
        {
            //Selection root = selection.Parent != null ? selection.Parent : selection;
            Selection root = selection.GetRoot();
            using (StringWriter sw = new StringWriter())
            using (HtmlTextWriter writer = new HtmlTextWriter(sw, ""))
            {
                for (int i = 0; i < root.Value.Length; i++)
                {
                    //check if enter a selection
                    bool enteredSelection = false;
                    foreach (Selection child in root.Children)
                    {
                        if (i == child.Off)
                        {
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "indianred");
                            writer.RenderBeginTag(HtmlTextWriterTag.Span);
                            //writer.Write(child.Value);
                            writer.Write(BuildVisualisationHelper(child, 0));
                            writer.RenderEndTag();
                            break;
                        }
                    }
                }
                return (wrap ? HTML_START : "") + sw.ToString() + (wrap ? HTML_END : "");
            }
            //return (wrap ? HTML_START : "")+BuildVisualisationHelper(selection.GetRoot(), 0) + (wrap ? HTML_END : "");
        }
        private string BuildVisualisationHelper(Selection selection,int depth)
        {
            using (StringWriter sw = new StringWriter())
            using (HtmlTextWriter writer = new HtmlTextWriter(sw, ""))
            {
                StringBuilder buf = new StringBuilder();
                string total = selection.Value;
                for (int i = 0; i < total.Length; i++)
                {
                    bool enteredSel = false;
                    foreach (Selection child in selection.Children)
                    {
                        if (child.Off == i)
                        {
                            //flush buf
                            sw.Write(buf.ToString());
                            buf.Clear();

                            enteredSel = true;
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, colours[depth%colours.Length]);
                            writer.RenderBeginTag(HtmlTextWriterTag.Span);
                            writer.Write(BuildVisualisationHelper(child,depth+1));
                            writer.RenderEndTag();

                            i += child.Value.Length;
                            break;
                        }
                    }
                    if (!enteredSel)
                    {
                        buf.Append(total[i]);
                    }
                }
                //flush buf
                sw.Write(buf.ToString());
                buf.Clear();

                return sw.ToString();
            }
        }
        private void _translator_OnDebug(Selection selection)
        {
            int selstart = selection.GetAbs();
            string total = _translator.Value;
            if (!fastMode)
            {
                System.Threading.Thread.Sleep(1000);
                this.Invoke(new Action(() =>
                {
                    if (!fastMode)
                    {
                        webBrowser1.DocumentText = BuildVisualisation(selection);
                    }
                    progressBar1.Value = (int)(selstart / (double)(total.Length) * 10000);
                }));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_translator.Value))
            {
                Clipboard.SetText(_translator.Value);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (autoMode)
            {
                button1_Click(null, null);
            }
        }
    }
}
