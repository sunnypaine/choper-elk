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
        [XmlElement("BasicHttpBinding")]
        public BasicHttpBindingConfig BasicHttpBinding { get; set; }

        [XmlElement("WSHttpBinding")]
        public WSHttpBindingConfig WSHttpBinding { get; set; }

        [XmlElement("NetTcpBinding")]
        public NetTcpBindingConfig NetTcpBinding { get; set; }
    }
}
