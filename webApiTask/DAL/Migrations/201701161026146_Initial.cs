namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Priority = c.Int(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                        IsNotify = c.Boolean(nullable: false),
                        NextNotifyTime = c.DateTime(),
                        ToDoList_Id = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ToDoLists", t => t.ToDoList_Id, cascadeDelete: true)
                .Index(t => t.ToDoList_Id);
            
            CreateTable(
                "dbo.ToDoLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        User_Id = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Password = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoLists", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ToDoItems", "ToDoList_Id", "dbo.ToDoLists");
            DropIndex("dbo.ToDoLists", new[] { "User_Id" });
            DropIndex("dbo.ToDoItems", new[] { "ToDoList_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.ToDoLists");
            DropTable("dbo.ToDoItems");
        }
    }
}
