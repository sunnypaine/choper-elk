using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示一个Configuration特性标记的类的字段可被ini配置信息自动赋值。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ValueAttribute : Attribute
    {
        private string key;
        /// <summary>
        /// 配置信息的键
        /// </summary>
        public string Key
        {
            get { return key; }
        }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Attributes.ValueAttribute 的新实例。
        /// </summary>
        public ValueAttribute()
        { }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Attributes.ValueAttribute 的新实例。
        /// </summary>
        /// <param name="key"></param>
        public ValueAttribute(string key)
        {
            this.key = key;
        }
    }
}
