using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk
{
    public static class ContainerFactory
    {
        /// <summary>
        /// 创建Bean容器
        /// </summary>
        /// <returns></returns>
        public static Container CreateContainer()
        {
            return new Container();
        }
    }
}
