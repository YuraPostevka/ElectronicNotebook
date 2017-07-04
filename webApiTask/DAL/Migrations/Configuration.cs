namespace DAL.Migrations
{
    using Common.Enum;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.MainContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //ToDoItem toDoItem1 = new ToDoItem()
            //{
            //    Text = "qwerty",
            //    Priority = 0,
            //    IsCompeted = true,
            //    IsNotify = true,
            //    NextNotifyTime = new DateTime(12, 01, 01)
            //};

            //ToDoList toDoList1 = new ToDoList()
            //{
            //    Name = "qwerty",
            //    User_Id = 1,
            //    Items = null
            //};
            //User user1 = new User()
            //{
            //    Email = "yurapostevka@gmail.com",
            //    Password = "000000",
            //    ToDoLists = new List<ToDoList>()
            //};
            //user1.ToDoLists.Add(toDoList1);


        }
    }
}
