namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Automatically_Generate_CreateDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ToDoItems", "Created", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ToDoLists", "Created", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Users", "Created", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ToDoLists", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ToDoItems", "Created", c => c.DateTime(nullable: false));
        }
    }
}
