using LibRgxC;
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
    public partial class TranslatorDisplay : Form
    {
        Translator _translator = null;
        public TranslatorDisplay()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _translator = new Translators.TestTranslator(textBox1.Text);
            _translator.OnDebug += _translator_OnDebug;
            new System.Threading.Thread(() => { _translator.Translate(); }).Start();
            
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
        private void _translator_OnDebug(Selection selection)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html white-space:pre><html><head><script>function scroll(){document.getElementById(\"sel\").scrollIntoView(true);}</script><style>body{font-family: Arial;white-space:PRE;font-size: 12px;}</style></head><body>");
            int selstart = selection.GetAbs();
            int len = selection.Len;
            string total = _translator.GetRoot().Value;
            for(int i = LastBefore(total,selstart,5,"\n"); i < total.Length; i++)
            {
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
                if (i == selstart + len-1)
                {
                    sb.Append(@"</span>");
                }
            }
            sb.Append("</body>\r\n</html>\r\n");


            this.Invoke(new Action(()=>{
                webBrowser1.DocumentText = sb.ToString();
                progressBar1.Value = (int)(selstart / (double)(total.Length) * 10000);
            }));
        }
    }
}
