using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Threading;
using System.Windows.Forms;


namespace OpenXML_and_XPath
{

    class XPathHandler
    {
        private static XPathHandler instance = null;

        Thread xmlDisplayThread2 = null;

        public string Path { get; set; }
        public string XPath { get; set; }

        PrintDataEventHandler printDataResult = null;
        PrintDataEventHandler printDataType = null;
        PrintDataEventHandler printDataLog = null;

        public event PrintDataEventHandler PrintDataResult
        {
            add { if (printDataResult == null) printDataResult += value; }
            remove { printDataResult -= value; }
        }
        public event PrintDataEventHandler PrintDataType
        {
            add { if (printDataType == null) printDataType += value; }
            remove { printDataType -= value; }
        }
        public event PrintDataEventHandler PrintDataLog
        {
            add { if (printDataLog == null) printDataLog += value; }
            remove { printDataLog -= value; }
        }


        protected XPathHandler()
        {
        }

        public static XPathHandler Instance()
        {
            if (instance == null)
            {
                instance = new XPathHandler();
            }
            return instance;
        }
        
       

        private void PrintDataResultEvent(string s)
        {
            printDataResult.Invoke(s);
        }
        private void PrintDataTypeEvent(string s)
        {
            printDataType.Invoke(s);
        }
        private void PrintDataLogEvent(string s)
        {
            printDataLog.Invoke(s);
        }


        public void CreateThread()
        {
            if (xmlDisplayThread2 != null)
            {
                xmlDisplayThread2.Abort();
            }
            xmlDisplayThread2 = new Thread(Parse);
            xmlDisplayThread2.Start();
        }

        private void Parse()
        {
            try
            {
                var document = new XPathDocument(Path);
                XPathNavigator navigator = document.CreateNavigator();

                XPathExpression expression = navigator.Compile(XPath);

                PrintDataTypeEvent((expression.ReturnType).ToString());

                PrintDataLogEvent("XPath processing...");

                switch (expression.ReturnType)
                {
                    case XPathResultType.Number:
                        PrintDataResultEvent(((double)navigator.Evaluate(expression)).ToString());
                        break;

                    case XPathResultType.NodeSet:
                        XPathNodeIterator iterator = navigator.Select(expression);
                        while (iterator.MoveNext())
                        {
                            XPathNavigator nav2 = iterator.Current.Clone();

                            PrintDataResultEvent(nav2.OuterXml);
                            PrintDataResultEvent("\n");
                        }
                        break;

                    case XPathResultType.Boolean:
                        if ((bool)navigator.Evaluate(expression))
                            PrintDataResultEvent("True!");
                        else
                            PrintDataResultEvent("False!");
                        break;

                    case XPathResultType.String:
                        PrintDataResultEvent((navigator.Evaluate(expression)).ToString());
                        break;
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
                if (XPath != "")
                    PrintDataLogEvent(ex.Message);
            }

        }



    }
}
