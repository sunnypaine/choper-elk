using Choper.Elk.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Test.BLL
{
    public interface IUserBusiness
    {
        List<User> FindAllUser();

        User FindUserById(string userId);
    }
}
