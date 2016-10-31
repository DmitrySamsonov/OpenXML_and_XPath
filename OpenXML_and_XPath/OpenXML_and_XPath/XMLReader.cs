using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Threading;

namespace OpenXML_and_XPath
{
    public delegate void PrintDataEventHandler(string s);

    class XMLReader
    {
        private static XMLReader instance = null;

        Thread xmlDisplayThread = null;

        public string Path { get; set; }

        PrintDataEventHandler printDataXML = null;
        PrintDataEventHandler printDataLog = null;

        public event PrintDataEventHandler PrintDataXML
        {
            add { if (printDataXML == null) printDataXML += value; }
            remove { printDataXML -= value; }
        }
        public event PrintDataEventHandler PrintDataLog
        {
            add { if (printDataLog == null) printDataLog += value; }
            remove { printDataLog -= value; }
        }


        protected XMLReader()
        {
        }

        public static XMLReader Instance()
        {
            if (instance == null)
            {
                instance = new XMLReader();
            }
            return instance;
        }



        private void PrintDataXMLEvent(string s)
        {
            printDataXML.Invoke(s);
        }
        private void PrintDataLogEvent(string s)
        {
            printDataLog.Invoke(s);
        }


        public void CreateThread()
        {
            if (xmlDisplayThread != null)
            {
                xmlDisplayThread.Abort();
            }
            xmlDisplayThread = new Thread(Parse);
            xmlDisplayThread.Start();
        }


        private void Parse()
        {
            try
            {
                PrintDataLogEvent("XML opening...");

                var document = new XPathDocument(Path);
                XPathNavigator navigator = document.CreateNavigator();

                XPathExpression expression = navigator.Compile("/");
                XPathNodeIterator iterator = navigator.Select(expression);

                if (iterator.MoveNext())
                {
                    XPathNavigator nav2 = iterator.Current.Clone();
                    PrintDataXMLEvent(nav2.OuterXml);
                }

                PrintDataLogEvent("");
            }
            catch (XmlException xExc)
            //Exception is thrown is there is an error in the Xml
            {
                PrintDataLogEvent(xExc.Message);
            }
            catch (Exception ex) //General exception
            {
                PrintDataLogEvent(ex.Message);
            }





            #region 2 вариант вывода
            // +  с памятью проблем нет.
            // -  проблема со временем: долго выводит на экран. 

            // XmlTextReader reader = new XmlTextReader(Path);
            // 
            // // ignore all white space nodes!!! IT'S FOR QUICKLY !
            // reader.WhitespaceHandling = WhitespaceHandling.None;
            // 
            // 
            // while (reader.Read())
            // {
            //     switch (reader.NodeType)
            //     {
            //         case XmlNodeType.Comment:
            //             PrintDataEvent(new string('\t', reader.Depth) + "Comment: " + reader.Name + " " + reader.Value + Environment.NewLine);
            //             break;
            // 
            //         case XmlNodeType.Element:
            //             PrintDataEvent(new string('\t', reader.Depth) + reader.Name + ":" + Environment.NewLine);
            // 
            //             if (reader.HasAttributes)
            //             {
            //                 while (reader.MoveToNextAttribute())
            //                 {
            //                     PrintDataEvent(new string('\t', reader.Depth) + "A: " + reader.Name + " " + reader.Value + Environment.NewLine);
            //                 }
            //             }
            //             break;
            // 
            //         case XmlNodeType.Text:
            //             PrintDataEvent(new string('\t', reader.Depth) + reader.Value + Environment.NewLine);
            //             break;
            // 
            //         default:
            //             break;
            //     }
            // }

            #endregion
        }
    }
}
