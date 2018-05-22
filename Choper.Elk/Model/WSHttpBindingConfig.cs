using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Choper.Elk.Model
{
    [Serializable]
    public class WSHttpBindingConfig
    {
        public bool AllowCookies { get; set; }

        public long CloseTimeout { get; set; }

        public long OpenTimeout { get; set; }

        public long SendTimeout { get; set; }

        public long ReceiveTimeout { get; set; }

        public bool BypassProxyOnLocal { get; set; }

        public bool TransactionFlow { get; set; }

        public long MaxBufferPoolSize { get; set; }

        public long MaxReceivedMessageSize { get; set; }

        public string TextEncoding { get; set; }

        public bool UseDefaultWebProxy { get; set; }

        public bool CrossDomainScriptAccessEnabled { get; set; }

        [XmlElement("ReaderQuotas")]
        public ReaderQuotasConfig ReaderQuotasConfig { get; set; }
    }
}
