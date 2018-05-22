using Choper.Elk.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Test.DAL
{
    public static class UserData
    {
        public static List<User> Users { get; set; }


        static UserData()
        {
            Users = new List<User>();
            for (int i = 1; i < 50; i++)
            {
                string id = i.ToString().PadLeft(3, '0');
                User u = new User();
                u.Id = Guid.NewGuid().ToString("N");
                u.Name = "Name" + id;
                u.Password = "Password" + id;
                u.Birthday = DateTime.Now;
                u.Flag = i % 7 == 0 ? false : true;

                Users.Add(u);
            }
        }

        public static bool Save(User user)
        {
            int index = Users.FindIndex(p => p.Equals(user.Name));
            if (index >0)
            {
                Users.Add(user);
                return true;
            }
            return false;
        }
    }
}
