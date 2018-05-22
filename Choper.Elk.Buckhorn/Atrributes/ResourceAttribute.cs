using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示字段可被自动注入值。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ResourceAttribute : Attribute
    {
        /// <summary>
        /// 字段的实例名称。
        /// </summary>
        public string Name
        { get; set; }


        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.ResourceAttribute 的新实例。
        /// </summary>
        public ResourceAttribute()
        { }
    }
}
