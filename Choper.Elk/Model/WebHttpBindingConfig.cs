using System;
using System.Xml.Serialization;

namespace Choper.Elk.Model
{
    [Serializable]
    public class WebHttpBindingConfig
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
    }
}
