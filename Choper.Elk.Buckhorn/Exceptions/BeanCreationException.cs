using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Exceptions
{
    /// <summary>
    /// 发生实例创建错误时引发的异常。
    /// </summary>
    public class BeanCreationException : Exception
    {
        public BeanCreationException(string message)
            : base(message)
        { }

        public BeanCreationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
