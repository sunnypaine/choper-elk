using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示方法的参数可以被自动注入值。
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterAttribute : ComponentAttribute
    {
        private string name;
        /// <summary>
        /// 参数的实例名称。
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.ParameterAttribute 的新实例。
        /// </summary>
        public ParameterAttribute()
        { }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.ParameterAttribute 的新实例。
        /// </summary>
        /// <param name="name">参数的实例名称。</param>
        public ParameterAttribute(string name)
        {
            this.name = name;
        }
    }
}
