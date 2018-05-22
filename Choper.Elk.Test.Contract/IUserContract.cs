using Choper.Elk.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Test.Contract
{
    [ServiceContract(Namespace = "http://www.choper.org")]
    public interface IUserContract
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "/User/QueryAllUser",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [return: MessageParameter(Name = "Users")]
        List<User> QueryAllUser();

        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "/User/{userId}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [return: MessageParameter(Name = "User")]
        User QueryUserById(string userId);
    }
}
