using Choper.Elk.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Test.DAL
{
    public interface IUserDataAccess
    {
        /// <summary>
        /// 查询：查询所有用户。
        /// </summary>
        /// <returns></returns>
        List<User> SelectAllUser();

        /// <summary>
        /// 查询：根据Id查询用户。
        /// </summary>
        /// <param name="id">用户Id。主键。</param>
        /// <returns></returns>
        User SelectUserById(string id);
    }
}
