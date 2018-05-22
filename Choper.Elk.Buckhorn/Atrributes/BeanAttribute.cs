using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示具有返回值的方法可以被自动实例化。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Serializable]
    public class BeanAttribute : Attribute
    {
        /// <summary>
        /// 实例的名称。
        /// </summary>
        public string Name
        { get; set; }


        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributess.BeanAttribute 类的新实例。
        /// </summary>
        public BeanAttribute()
        { }
    }
}
