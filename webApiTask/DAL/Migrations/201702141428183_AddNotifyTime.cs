namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifyTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoItems", "NotifyTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoItems", "NotifyTime");
        }
    }
}
