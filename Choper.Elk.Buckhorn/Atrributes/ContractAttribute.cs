using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示表示WCF协议接口的实现类可被自动实例化。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ContractAttribute : ComponentAttribute
    {
        private string name;
        /// <summary>
        /// 实例的名称。
        /// </summary>
        public string Name
        {
            get { return name; }
        }


        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.ContractAttribute 类的新实例。
        /// </summary>
        public ContractAttribute() : base()
        { }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.ContractAttribute 类的新实例。
        /// </summary>
        /// <param name="name">实例的名称。</param>
        public ContractAttribute(string name) : base(name)
        {
            this.name = name;
        }
    }
}
