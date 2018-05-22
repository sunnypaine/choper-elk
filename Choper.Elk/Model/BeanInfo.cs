using Choper.Elk.Buckhorn.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Model
{
    /// <summary>
    /// 实例信息。
    /// </summary>
    public class BeanInfo
    {
        /// <summary>
        /// 实例。
        /// </summary>
        public object Bean { get; set; }

        /// <summary>
        /// 容器的特性类型。
        /// </summary>
        public AtrributeType AtrributeType { get; set; }
    }
}
