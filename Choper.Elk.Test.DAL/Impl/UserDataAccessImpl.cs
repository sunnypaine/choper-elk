using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Choper.Elk.Buckhorn.Atrributes;
using Choper.Elk.Test.Model;

namespace Choper.Elk.Test.DAL.Impl
{
    [DataAccess("userDataAccess")]
    public class UserDataAccessImpl : IUserDataAccess
    {
        public List<User> SelectAllUser()
        {
            return UserData.Users;
        }

        public User SelectUserById(string id)
        {
            return UserData.Users.Where(p => p.Id.Equals(id)).FirstOrDefault();
        }
    }
}
