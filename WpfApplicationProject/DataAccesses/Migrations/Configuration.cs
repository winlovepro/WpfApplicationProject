namespace WpfApplicationProject.DataAccesses.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WpfApplicationProject.DataAccesses.EF;
    using WpfApplicationProject.DataAccesses.Entities;
    using WpfApplicationProject.Utilities;

    internal sealed class Configuration : DbMigrationsConfiguration<WpfDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WpfDBContext context)
        {
            IList<User> users = new List<User>()
            {
                new User()
                {
                    Id =  SystemMethods.GetNewID(),
                    Firstname  = "admin",
                    Lastname = "admin",
                    Username = "admin",
                    Password = SystemMethods.GetHash("admin"),
                    Active = true,
                    Description ="Administrator"
                },
                new User()
                {
                    Id =   SystemMethods.GetNewID(),
                    Firstname  = "Lê Trọng",
                    Lastname = "Thắng",
                    Username = "thanglt",
                    Password = SystemMethods.GetHash("thanglt"),
                    Active = true,
                    Description ="Administrator"
                },
                new User()
                {
                    Id =   SystemMethods.GetNewID(),
                    Firstname  = "Lê Hoàng",
                    Lastname = "Quân",
                    Username = "quanlh",
                    Password = SystemMethods.GetHash("quanlh"),
                    Active = true,
                    Description ="Modector"
                },
                new User()
                {
                    Id =   SystemMethods.GetNewID(),
                    Firstname  = "user",
                    Lastname = "common",
                    Username = "user",
                    Password = SystemMethods.GetHash("user"),
                    Active = false,
                    Description ="user"
                }
            };

            context.Users.AddRange(users);

            base.Seed(context);

            context.SaveChanges();
        }
    }
}
