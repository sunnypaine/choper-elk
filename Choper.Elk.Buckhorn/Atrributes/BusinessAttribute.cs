using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示表示业务逻辑层的类可被自动实例化。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BusinessAttribute : ComponentAttribute
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
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.BusinessAttribute 类的新实例。
        /// </summary>
        public BusinessAttribute():base()
        { }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.BusinessAttribute 类的新实例。
        /// </summary>
        /// <param name="name">实例的名称。</param>
        public BusinessAttribute(string name) : base(name)
        {
            this.name = name;
        }
    }
}
