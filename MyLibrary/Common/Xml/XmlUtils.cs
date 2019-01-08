using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CommonLib.Xml
{
    /// <summary>
    /// Static Xml related utility functions.
    /// </summary>
    public sealed class XmlUtils
    {
        /// <summary>
        ///Default constructor.
        /// </summary>
        private XmlUtils()
        {
        }

        /// <summary>
        /// 获取文件路径并返回xmldocument
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns>加载的 XML document.</returns>
        public static XmlDocument LoadXMLFromFile(string file)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(file);
            }
            catch (XmlException)
            {
                return null;
            }
            return xmldoc;
        }


        /// <summary>
        /// Remove all children (but not attributes) from specified node
        /// </summary>
        /// <param name="n">Node to remove children from</param>
        public static void RemoveAllChildrenFrom(XmlNode n)
        {
            while (n.HasChildNodes) n.RemoveChild(n.FirstChild);
        }


        /// <summary>
        /// 获取xNav的Current节点的属性值。 如果attrName不存在，则抛出异常。
        /// </summary>
        /// <param name="xNav">Instance of XPath navigator.</param>
        /// <param name="attrName">Attribute name.</param>
        /// <returns>Attribute Value.</returns>
        public static string GetAttributeValue(XPathNavigator xNav, string attrName)
        {
            string retVal = xNav.GetAttribute(attrName, "");
            if (retVal == string.Empty)
                throw new Exception("GetAttributeValue:: Could not find Required attribute: " + attrName + " in node:" + xNav.Value);
            else
                return retVal;
        }


        /// <summary>
        /// Gets the attribute value for the Current node of xNav_. Returns defaultValue if attrName_ does not exist.
        /// </summary>
        /// <param name="xNav">Instance of XPath navigator.</param>
        /// <param name="attrName">Attribute name.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Attribute value.</returns>
        public static string GetAttributeValue(XPathNavigator xNav, string attrName, string defaultValue)
        {
            string retVal = xNav.GetAttribute(attrName, "");
            if (retVal == string.Empty)
                return defaultValue;
            else
                return retVal;
        }


        /// <summary>
        /// 将xml的片段转换为xml节点
        /// </summary>
        /// <param name="xmlFragment">以外部元素开头的xml片段</param>
        /// <returns>新的xml节点</returns>
        public static XmlNode FragmentToNode(string xmlFragment)
        {
            XmlDocument xd = new XmlDocument();

            using (System.IO.StringReader sr = new StringReader(xmlFragment))
            {
                xd.Load(sr);
            }

            return xd.FirstChild;
        }


        /// <summary>
        /// xml转义.
        /// </summary>
        /// <param name="xml">要转义的XML内容字符串。</param>
        /// <returns>转义后的XML内容字符串。</returns>
        public static string EscapeXml(string xml)
        {
            if (xml.IndexOf("&") >= 0)
                xml = xml.Replace("&", "&amp;");

            if (xml.IndexOf("'") >= 0)
                xml = xml.Replace("'", "&apos;");

            if (xml.IndexOf("\"") >= 0)
                xml = xml.Replace("\"", "&quot;");

            if (xml.IndexOf("<") >= 0)
                xml.Replace("<", "&lt;");

            if (xml.IndexOf(">") >= 0)
                xml.Replace(">", "&gt;");

            return xml;
        }


        /// <summary>
        /// Pretty Print the input XML string, such as adding indentations to each level of elements
        /// and carriage return to each line
        /// </summary>
        /// <param name="xmlText">XML content.</param>
        /// <returns>New formatted XML string</returns>
        public static String FormatNicely(String xmlText)
        {
            if (xmlText == null || xmlText.Trim().Length == 0)
                return "";

            String result = "";

            MemoryStream memStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(memStream, Encoding.Unicode);
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                xmlDoc.LoadXml(xmlText);

                xmlWriter.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                xmlDoc.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
                memStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                memStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader streamReader = new StreamReader(memStream);

                // Extract the text from the StreamReader.
                String FormattedXML = streamReader.ReadToEnd();

                result = FormattedXML;
            }
            catch (Exception)
            {
                // Return the original unchanged.
                result = xmlText;
            }
            finally
            {
                memStream.Close();
                xmlWriter.Close();
            }
            return result;
        }


        /// <summary>
        /// 转换XML
        /// </summary>
        /// <param name="inXml">The in XML.</param>
        /// <param name="styleSheet">The style sheet.</param>
        /// <param name="outXml">The out XML.</param>
        /// <returns>Transformed XML.</returns>
        public static TextWriter TransformXml(TextReader inXml, TextReader styleSheet, TextWriter outXml)
        {
            if (null == inXml || null == styleSheet)
                return outXml;
   
            try
            {
                XslCompiledTransform xslt = new XslCompiledTransform();
                XsltSettings settings = new XsltSettings(false, false); //script support disabled
                using (XmlReader sheetReader = XmlReader.Create(styleSheet))
                    xslt.Load(sheetReader, settings, null); //resolver set to null

                using (XmlReader inReader = XmlReader.Create(inXml))
                    xslt.Transform(inReader, null, outXml); //set XsltArgumentList to null
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error occured while performing xsl tranformation", e);
            }
            return outXml;
        }


        /// <summary>
        /// Generates html by transforming the xml to html
        /// using xsl file specified.
        /// </summary>
        /// <param name="xmlToTransform">The xml to transform to html.</param>
        /// <param name="pathToXsl">The path to the xsl file to use for
        /// the transformation.</param>
        /// <returns>An html string if correctly transformed, or an empty string
        /// if there was some error.</returns>
        public static string TransformXml(string xmlToTransform, string pathToXsl)
        {
            if (xmlToTransform == null || xmlToTransform.Length == 0 || !System.IO.File.Exists(pathToXsl))
                return "";

            string rc = "";
            try
            {
                using (TextReader styleSheet = new StreamReader(new FileStream(
                  pathToXsl, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    // 不要在StringReader / Writer上调用close
                    TextReader inXml = new StringReader(xmlToTransform);
                    TextWriter outXml = new StringWriter();

                    TransformXml(inXml, styleSheet, outXml);

                    rc = outXml.ToString();
                }
            }
            catch (Exception)
            {
                rc = "";
            }
            return rc;
        }


        /// <summary>
        /// 使用Xml序列化将对象序列化为xml。
        /// obj必须具有用于序列化的xml属性。
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>XML contents representing the serialized object.</returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
                return string.Empty;

            XmlSerializer ser = new XmlSerializer(obj.GetType());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(sb);
            ser.Serialize(writer, obj);
            string xml = sb.ToString();
            return xml;
        }

    }
}
