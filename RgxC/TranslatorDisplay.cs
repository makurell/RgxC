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

        private void _translator_OnDebug(Selection selection)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html white-space:pre>\r\n<html>\r\n<head>\r\n<style>\r\nbody{\r\nfont-family: Arial;white-space:PRE;\r\nfont-size: 12px;\r\n}\r\n</style>\r\n</head>\r\n<body>");
            int selstart = selection.GetAbs();
            int len = selection.Len;
            string total = _translator.GetRoot().Value;
            for(int i = 0; i < total.Length; i++)
            {
                if (i == selstart)
                {
                    sb.Append(@"<span style=""background-color:lightgray"">");
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
            webBrowser1.DocumentText = sb.ToString();
        }
    }
}
