using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Choper.Elk.Buckhorn.Atrributes;
using Choper.Elk.Test.BLL;
using Choper.Elk.Test.Model;

namespace Choper.Elk.Test.Contract.Impl
{
    [Contract("userContract")]
    public class UserContractImpl : IUserContract
    {
        [Resource(Name = "userBusiness")]
        private IUserBusiness userBusiness;



        [return: MessageParameter(Name = "Users")]
        public List<User> QueryAllUser()
        {
            return this.userBusiness.FindAllUser();
        }

        [return: MessageParameter(Name = "User")]
        public User QueryUserById(string userId)
        {
            return this.userBusiness.FindUserById(userId);
        }
    }
}
