using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Choper.Elk.Model
{
    [Serializable]
    public class NetTcpBindingConfig
    {
        /// <summary>
        /// 关闭连接超时时间。单位：秒。
        /// </summary>
        public long CloseTimeout { get; set; }

        /// <summary>
        /// 打开连接超时时间。单位：秒。
        /// </summary>
        public long OpenTimeout { get; set; }

        /// <summary>
        /// 发送数据超时时间。单位：秒。
        /// </summary>
        public long SendTimeout { get; set; }

        /// <summary>
        /// 接收数据超时时间。单位：秒。
        /// </summary>
        public long ReceiveTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TransactionFlow { get; set; }

        public long MaxBufferPoolSize { get; set; }

        public long MaxReceivedMessageSize { get; set; }

        public int MaxBufferSize { get; set; }

        public int ListenBacklog { get; set; }

        public int MaxConnections { get; set; }

        [XmlElement("ReaderQuotas")]
        public ReaderQuotasConfig ReaderQuotasConfig { get; set; }
    }
}
