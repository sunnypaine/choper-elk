using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Exceptions
{
    /// <summary>
    /// 发生实例找不到时引发的异常。
    /// </summary>
    public class BeanNotFoundException : KeyNotFoundException
    {
        public BeanNotFoundException(string message)
            : base(message)
        { }

        public BeanNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
