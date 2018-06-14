using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Choper.Elk.Model
{
    [Serializable]
    public class WcfConfig
    {
        /// <summary>
        /// 绑定类型。BasicHttpBinding：支持soap1.0的协议；WebHttpBinding：可用于为通过 HTTP 请求；NetTcpBinding：tcp协议。
        /// </summary>
        [XmlElement("BindingType")]
        public string BindingType { get; set; }

        [XmlElement("BasicHttpBinding")]
        public BasicHttpBindingConfig BasicHttpBinding { get; set; }

        [XmlElement("WebHttpBinding")]
        public WebHttpBindingConfig WebHttpBinding { get; set; }

        [XmlElement("NetTcpBinding")]
        public NetTcpBindingConfig NetTcpBinding { get; set; }
    }
}
