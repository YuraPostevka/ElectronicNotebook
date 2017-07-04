namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addToDoListTag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoListsTags",
                c => new
                    {
                        TagId = c.Int(nullable: false),
                        ToDoListId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagId, t.ToDoListId })
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .ForeignKey("dbo.ToDoLists", t => t.ToDoListId, cascadeDelete: true)
                .Index(t => t.TagId)
                .Index(t => t.ToDoListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoListsTags", "ToDoListId", "dbo.ToDoLists");
            DropForeignKey("dbo.ToDoListsTags", "TagId", "dbo.Tags");
            DropIndex("dbo.ToDoListsTags", new[] { "ToDoListId" });
            DropIndex("dbo.ToDoListsTags", new[] { "TagId" });
            DropTable("dbo.ToDoListsTags");
        }
    }
}
