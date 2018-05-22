using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    /// <summary>
    /// 指示一个用于配置信息的类可以被自动实例化。。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : ComponentAttribute
    {
        private string name;
        /// <summary>
        /// 实例的名称。
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private string path;
        /// <summary>
        /// 配置文件的路径。
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.ConfigurationAttribute 的新实例。
        /// </summary>
        public ConfigurationAttribute() : base()
        { }

        /// <summary>
        /// 初始化 Choper.Elk.Buckhorn.Atrributes.ConfigurationAttribute 的新实例。
        /// </summary>
        /// <param name="name">实例的名称。</param>
        /// <param name="path">配置文件的路径。</param>
        public ConfigurationAttribute(string name, string path = null) : base(name)
        {
            this.name = name;
            this.path = path;
        }
    }
}
