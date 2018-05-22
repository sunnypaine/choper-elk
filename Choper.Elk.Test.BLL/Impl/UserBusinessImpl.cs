using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Choper.Elk.Buckhorn.Atrributes;
using Choper.Elk.Test.DAL;
using Choper.Elk.Test.Model;

namespace Choper.Elk.Test.BLL.Impl
{
    [Business("userBusiness")]
    public class UserBusinessImpl : IUserBusiness
    {
        [Resource(Name = "userDataAccess")]
        private IUserDataAccess userDataAccess;


        public List<User> FindAllUser()
        {
            return this.userDataAccess.SelectAllUser();
        }

        public User FindUserById(string userId)
        {
            return this.userDataAccess.SelectUserById(userId);
        }
    }
}
