using GroupSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

class Admin
{
    public static void InsertUser(string userName)
    {
        using (TransactionScope scope = new TransactionScope())
        {
            using (SystemEntities context = new SystemEntities())
            {
                Group adminGroup = new Group { Name = "Admins" };
                if (context.Groups.Count(x => x.Name == "Admins") == 0)
                {
                    context.Groups.Add(adminGroup);
                    context.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    if (context.Users.Count(x => x.UserName == userName) > 0)
                    {
                        Console.WriteLine("User already exists.");
                        scope.Dispose();
                    }

                    Group currentGroup = context.Groups
                        .Where(x => x.Name == "Admins").First();

                    User newUser = new User()
                    {
                        UserName = userName,
                        GroupId = currentGroup.GroupId
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();
                    scope.Complete();
                }
            }
        }

    }

    static void Main()
    {
        InsertUser("Pesho");
    }
}