using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示表示数据访问层的类可被自动实例化。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataAccessAttribute : ComponentAttribute
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
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.DataAccessAttribute 类的新实例。
        /// </summary>
        public DataAccessAttribute() : base()
        { }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.DataAccessAttribute 类的新实例。
        /// </summary>
        /// <param name="name">实例的名称。</param>
        public DataAccessAttribute(string name) : base(name)
        {
            this.name = name;
        }
    }
}
