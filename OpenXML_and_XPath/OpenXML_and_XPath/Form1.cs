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
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = openFileDialog1.FileName;


                Thread myThread = new Thread(new ParameterizedThreadStart(Method));
                myThread.Start(path);
            }
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            richTextBox1.AppendText(value);
        }


        void Method(object path)
        {
            XmlTextReader reader = new XmlTextReader(path.ToString());

            
            // ignore all white space nodes!!! IT'S FOR QUICKLY !
            reader.WhitespaceHandling = WhitespaceHandling.None;


            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Comment:
                        //and add the first (root) node
                        //treeView1.Nodes.Add(new TreeNode(new string(' ', 4 * reader.Depth) + "Comment: " + reader.Name + " " + reader.Value + " + Numb: " + reader.LineNumber + " Pos: " + reader.LocalName));
                        AppendTextBox(new string('\t', reader.Depth) + "Comment: " + reader.Name + " " + reader.Value + Environment.NewLine);
                        //richTextBox1.Text += new string('\t', reader.Depth) + "Comment: " + reader.Name + " " + reader.Value + Environment.NewLine;
                        break;

                    case XmlNodeType.Element:
                        //and add the first (root) node
                        //treeView1.Nodes.Add(new TreeNode(/*"7" +*/new string(' ', 4 * reader.Depth) + reader.Name + " + Numb: " + reader.LineNumber + " Pos: " + reader.LocalName));
                        AppendTextBox( /*"7" +*/new string('\t', reader.Depth) + reader.Name + ":" + Environment.NewLine);
                        //richTextBox1.Text += /*"7" +*/new string('\t', reader.Depth) + reader.Name + ":" + Environment.NewLine;


                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                //and add the first (root) node
                                //treeView1.Nodes.Add(new TreeNode(new string(' ', 4 * reader.Depth) + "A: " + reader.Name + " " + reader.Value + " + Numb: " + reader.LineNumber + " Pos: " + reader.LocalName));
                                AppendTextBox( new string('\t', reader.Depth) + "A: " + reader.Name + " " + reader.Value + Environment.NewLine);
                                //richTextBox1.Text += new string('\t', reader.Depth) + "A: " + reader.Name + " " + reader.Value + Environment.NewLine;
                            }
                        }

                        
                        //TEXT BOX НАКРЫЛСЯ МЕДНЫМ ТАЗИКОМ!


                        break;

                    //case XmlNodeType.EndElement:
                    //    //and add the first (root) node
                    //    //treeView1.Nodes.Add(new TreeNode("8" + reader.Name + "------DEAPTH:  " + reader.LocalName));
                    //    //tree_Node = treeNode.PrevNode;
                    //    break;

                    case XmlNodeType.Text:
                        //and add the first (root) node
                        //treeView1.Nodes.Add(new TreeNode(/*"16" +*/new string(' ', 4 * reader.Depth) + reader.Value + " + Numb: " + reader.LineNumber + " Pos: " + reader.LocalName));

                        //tree_Node = treeNode.PrevNode;
                        //poisk(reader, tree_Node);

                        AppendTextBox(new string('\t', reader.Depth) + reader.Value + Environment.NewLine);
                        //richTextBox1.Text += new string('\t', reader.Depth) + reader.Value + Environment.NewLine;
                        break;

                    default:
                        break;
                }

            }

            

        }
    }
}
