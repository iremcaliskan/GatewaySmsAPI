using System.Collections.Generic;
using System.Linq;

namespace GatewaySmsAPI.DbOperations
{
    public class DataGenerator
    {
        public static void Initialize()
        {
            using (var context = new DemoDBContext())
            {
                if (context.Users.Any())
                {
                    return;
                }

                var userList = new List<User>()
                {
                    new User { UserId = 1, UserType = "Office", FirstName = "İrem", LastName = "Çalışkan", PhoneNumber = 901234567891, Sender = "Demo Project" },
                    new User { UserId = 2, UserType = "Service", FirstName = "İrem", LastName = "Çalışkan", PhoneNumber = 901234567891, Sender = "Demo Project" },
                    new User { UserId = 3, UserType = "Customer", FirstName = "İrem", LastName = "Çalışkan", PhoneNumber = 901234567891, Sender = "Demo Project" },
                };

                context.Users.AddRange(userList);
                context.SaveChanges();
            }
        }
    }
}