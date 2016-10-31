using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Threading;

namespace OpenXML_and_XPath
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void openXMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBoxShowXML.Clear();

                XMLReader xmlreader = XMLReader.Instance();
                xmlreader.Path = openFileDialog1.FileName;

                xmlreader.PrintDataXML += new PrintDataEventHandler(AppendRichBoxShowXML_Handler);
                xmlreader.PrintDataLog += new PrintDataEventHandler(AppendLogBox_Handler);

                xmlreader.CreateThread();
            }
        }

        private void textBoxInputXPath_KeyUp(object sender, KeyEventArgs e)
        {
            richTextBoxShowResult.Clear();
            label3.Text = "";

            XPathHandler xpathHandler = XPathHandler.Instance();
            xpathHandler.Path = openFileDialog1.FileName;
            xpathHandler.XPath = textBoxInputXPath.Text;

            xpathHandler.PrintDataResult += new PrintDataEventHandler(AppendTextBoxShowResult_Handler);
            xpathHandler.PrintDataType += new PrintDataEventHandler(AppendLabelBox_Handler);
            xpathHandler.PrintDataLog += new PrintDataEventHandler(AppendLogBox_Handler);

            xpathHandler.CreateThread();
        }


        public void AppendRichBoxShowXML_Handler(string value)
        {
            try
            {
                if (richTextBoxShowXML.InvokeRequired)
                {
                    richTextBoxShowXML.Invoke(new Action<string>(AppendRichBoxShowXML_Handler), new object[] { value });
                    return;
                }
                richTextBoxShowXML.AppendText(value);
            }
            catch (ThreadInterruptedException)
            {
                textBoxLog.Text = "operation was interrupted";
            }
            catch (Exception ex) //General exception
            {
                textBoxLog.Text = ex.Message;
            }
        }

        public void AppendTextBoxShowResult_Handler(string value)
        {
            try
            {
                if (richTextBoxShowResult.InvokeRequired)
                {
                    richTextBoxShowResult.Invoke(new Action<string>(AppendTextBoxShowResult_Handler), new object[] { value });
                    return;
                }
                richTextBoxShowResult.AppendText(value);
            }
            catch (ThreadInterruptedException)
            {
                textBoxLog.Text = "operation was interrupted";
            }
            catch (Exception ex) //General exception
            {
                textBoxLog.Text = ex.Message;
            }
        }

        public void AppendLabelBox_Handler(string value)
        {
            try
            {
                if (label3.InvokeRequired)
                {
                    label3.Invoke(new Action<string>(AppendLabelBox_Handler), new object[] { value });
                    return;
                }
                label3.Text = value;
                richTextBoxShowResult.Clear();
            }
            catch (ThreadInterruptedException)
            {
                textBoxLog.Text = "operation was interrupted";
            }
            catch (Exception ex) //General exception
            {
                textBoxLog.Text = ex.Message;
            }
        }

        public void AppendLogBox_Handler(string value)
        {
            try
            {
                if (label3.InvokeRequired)
                {
                    label3.Invoke(new Action<string>(AppendLogBox_Handler), new object[] { value });
                    return;
                }
                textBoxLog.Text = value;
            }
            catch (ThreadInterruptedException)
            {
                textBoxLog.Text = "operation was interrupted";
            }
            catch (Exception ex) //General exception
            {
                textBoxLog.Text = ex.Message;
            }
        }

    }
}
