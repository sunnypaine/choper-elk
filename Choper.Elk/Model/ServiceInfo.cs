using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Model
{
    /// <summary>
    /// WCF服务信息。
    /// </summary>
    public class ServiceInfo
    {
        /// <summary>
        /// 服务的接口名称
        /// </summary>
        public string ContractName { get; set; }

        /// <summary>
        /// 服务的接口所在的dll名称。
        /// </summary>
        public string ContractDll { get; set; }

        /// <summary>
        /// 服务的接口类型。
        /// </summary>
        public Type ContractType { get; set; }


        /// <summary>
        /// 服务实现类的名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务实现类的所在的dll名称
        /// </summary>
        public string ServiceDll { get; set; }

        /// <summary>
        /// 服务实现类的类型。
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// 服务寄宿者。
        /// </summary>
        public ServiceHost Host { get; set; }
    }
}
