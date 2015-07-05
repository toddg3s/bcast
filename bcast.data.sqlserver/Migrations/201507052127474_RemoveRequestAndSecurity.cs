namespace bcast.data.sqlserver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequestAndSecurity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Sec_id", "dbo.SecurityContexts");
            DropIndex("dbo.Items", new[] { "Sec_id" });
            AddColumn("dbo.Items", "Timestamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.Items", "Immediate");
            DropColumn("dbo.Items", "Sec_id");
            DropTable("dbo.SecurityContexts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SecurityContexts",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        account = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Items", "Sec_id", c => c.String(maxLength: 128));
            AddColumn("dbo.Items", "Immediate", c => c.Boolean());
            DropColumn("dbo.Items", "Timestamp");
            CreateIndex("dbo.Items", "Sec_id");
            AddForeignKey("dbo.Items", "Sec_id", "dbo.SecurityContexts", "id");
        }
    }
}
