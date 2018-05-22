using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Test.Model
{
    [DataContract]
    [Serializable]
    public class User
    {
        /// <summary>
        /// Id。
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// 生日。
        /// </summary>
        [DataMember]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 数据状态。true：正常；false：删除。
        /// </summary>
        [DataMember]
        public bool Flag { get; set; }
    }
}
